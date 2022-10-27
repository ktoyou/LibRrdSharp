using System.Diagnostics;
using System.Drawing;
using LibRrd;
using LibRrd.Archive;
using LibRrd.DataSources;
using LibRrd.Graph;
using LibRrd.Graph.Fonts;


RRD.RRD_PATH = @"D:\1\rrdtool.exe";
var rrd = RRD.Create("system.rrd", 2, new List<IDataSource>()
{
    new DS("load", DataType.GAUGE, 20, -1000, 1000)
}, new List<IRraArchive>()
{
    new RRA(RraType.LAST, 2592000, 0.5),
    new RRA(RraType.AVERAGE, 2592000, 0.5),
    new RRA(RraType.MIN, 2592000, 0.5),
    new RRA(RraType.MAX, 2592000, 0.5)
});

var cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

while (true)
{
    var processorLoad = cpu.NextValue();
    rrd.Update(new List<double>() {processorLoad}, DateTime.Now);
    
    RenderGraph();

    await Task.Delay(2000);
}

void RenderGraph()
{
    var graph = new Graph(800, 360, UnixTimeStampToDateTime(DateTimeOffset.Now.ToUnixTimeSeconds() - 300), DateTime.Now)
    {
        Title = "System Information",
        File = "cpu.png",
        Watermark = "Monitoring",
        TitleFont = new TitleFont("Ubuntu Mono Medium", 110),
        WatermarkFont = new WatermarkFont("Ubuntu Mono Medium", 20),
        DefaultFont = new DefaultFont("Ubuntu Mono Medium", 10)
    };
    graph.Defs.Add(new Def(rrd, rrd.GetDataSourceByName("load"), RraType.LAST, "load_def"));
    graph.Cdefs.Add(new Cdef("load_cdef", $"load_def,UN,0,load_def,IF"));
    graph.Shapes.Add(new Line(graph.Defs.First(elem => elem.Name == "load_def"), Color.Brown, 1, "CpuLoad"));
    graph.Comments.Add(new Comment("Cur".PadLeft(35)));
    graph.Comments.Add(new Comment("Avg".PadLeft(25)));
    graph.Comments.Add(new Comment("Min".PadLeft(26)));
    graph.Comments.Add(new Comment("Max\\n".PadLeft(28)));
    graph.Gprints.Add(new Gprint(graph.Defs.First(elem => elem.Name == "load_def"), RraType.LAST, "%6.2lf %%"));
    graph.Gprints.Add(new Gprint(graph.Defs.First(elem => elem.Name == "load_def"), RraType.AVERAGE, "%6.2lf %%"));
    graph.Gprints.Add(new Gprint(graph.Defs.First(elem => elem.Name == "load_def"), RraType.MIN, "%6.2lf %%"));
    graph.Gprints.Add(new Gprint(graph.Defs.First(elem => elem.Name == "load_def"), RraType.MAX, "%6.2lf %%"));

    graph.Render();
}

static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
{
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
    return dateTime;
}