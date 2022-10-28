using System.Diagnostics;
using System.Drawing;
using LibRrd;
using LibRrd.Archive;
using LibRrd.DataSources;
using LibRrd.Graph;
using LibRrd.Graph.Fonts;


const int min = int.MinValue;
const int max = int.MaxValue;
const int heartbeat = 20;
const int rraLength = 2592000;
const int step = 2;
const double xff = 0.5;

RRD.RRD_PATH = @"D:\1\rrdtool.exe";

var rrd = RRD.Create("system.rrd", step, new List<IDataSource>()
{
    new DS("cpu_load", DataType.GAUGE, heartbeat, min, max),
    new DS("memory_load", DataType.GAUGE, heartbeat, min, max)
}, new List<IRraArchive>()
{
    new RRA(RraType.LAST, rraLength, xff),
    new RRA(RraType.AVERAGE, rraLength, xff),
    new RRA(RraType.MIN, rraLength, xff),
    new RRA(RraType.MAX, rraLength, xff)
});

var cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
var memory = new PerformanceCounter("Memory", "Available MBytes");

while (true)
{
    var processorLoad = cpu.NextValue();
    var avaibleMemory = memory.NextValue() / 1000; 
    
    rrd.Update(new List<double>() {processorLoad, avaibleMemory}, DateTime.Now);
    
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
        TitleFont = new TitleFont("Ubuntu Mono Medium", 15),
        WatermarkFont = new WatermarkFont("Ubuntu Mono Medium", 10),
        DefaultFont = new DefaultFont("Ubuntu Mono Medium", 10)
    };
    graph.Defs.Add(new Def(rrd, rrd.GetDataSourceByName("cpu_load"), RraType.LAST, "load_def"));
    graph.Defs.Add(new Def(rrd, rrd.GetDataSourceByName("memory_load"), RraType.LAST, "memory_def"));
    graph.Cdefs.Add(new Cdef("load_cdef", $"load_def,UN,0,load_def,IF"));
    graph.Cdefs.Add(new Cdef("memory_cdef", $"memory_def,UN,0,memory_def,IF"));
    
    graph.Legend.Add(new Comment("Cur".PadLeft(23)));
    graph.Legend.Add(new Comment("Avg".PadLeft(9)));
    graph.Legend.Add(new Comment("Min".PadLeft(8)));
    graph.Legend.Add(new Comment("Max\\n".PadLeft(9)));
    
    graph.Legend.Add(new Line(graph.Defs.First(elem => elem.Name == "load_def"), Color.Brown, 1, "Cpu load"));
    graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "load_def"), RraType.LAST, "%6.2lf %%".PadLeft(15)));
    graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "load_def"), RraType.AVERAGE, "%6.2lf %%"));
    graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "load_def"), RraType.MIN, "%6.2lf %%"));
    graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "load_def"), RraType.MAX, "%6.2lf %%\\n"));
    
    graph.Legend.Add(new Line(graph.Defs.First(elem => elem.Name == "memory_def"), Color.Goldenrod, 1, "Available memory"));
    graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "memory_def"), RraType.LAST, "%6.2lf GB"));
    graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "memory_def"), RraType.AVERAGE, "%6.2lf GB"));
    graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "memory_def"), RraType.MIN, "%6.2lf GB"));
    graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "memory_def"), RraType.MAX, "%6.2lf GB\\n"));

    graph.Render();
}

static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
{
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
    return dateTime;
}