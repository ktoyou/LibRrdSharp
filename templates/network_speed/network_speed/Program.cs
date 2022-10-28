using System.Drawing;
using LibRrd;
using LibRrd.Archive;
using LibRrd.DataSources;
using LibRrd.Graph;
using LibRrd.Graph.Fonts;
using network_speed;

const string interfaceName = "Ethernet";
const int interval = 2000;
RRD.RRD_PATH = @"D:\1\rrdtool.exe";

var network = new Network();
var rrd = RRD.Create("speed.rrd", interval / 1000, new List<IDataSource>()
{
    new DS("rx", DataType.GAUGE, 20, int.MinValue, int.MaxValue)
}, new List<IRraArchive>()
{
    new RRA(RraType.MAX, 43200, 0.5),
    new RRA(RraType.AVERAGE, 43200, 0.5),
    new RRA(RraType.MIN, 43200, 0.5),
    new RRA(RraType.LAST, 43200, 0.5),
});

while (true)
{
    var speed = await network.GetCurrentInterfaceSpeedAsync("Ethernet", interval);
    rrd.Update(new[] { (double)speed }, DateTime.Now);
    GenerateGraph();
}

void GenerateGraph()
{
    var graph = new Graph(800, 360, UnixTimeStampToDateTime(DateTimeOffset.Now.ToUnixTimeSeconds() - 3600), DateTime.Now)
    {
        DefaultFont = new DefaultFont("Ubuntu Mono Medium", 12),
        File = "speed.png",
        Title = "Speed in the last hour"
    };


    var rxDef = new Def(rrd, rrd.GetDataSourceByName("rx"), RraType.LAST, "rx_def");
    graph.Defs.Add(rxDef);
    graph.Cdefs.Add(new Cdef("rx_cdef", "rx_def,-8,*"));
    
    graph.Legend.Add(new Comment("Cur".PadLeft(15)));
    graph.Legend.Add(new Comment("Avg".PadLeft(6)));
    graph.Legend.Add(new Comment("Min".PadLeft(6)));
    graph.Legend.Add(new Comment("Max\\n".PadLeft(14)));

    graph.Legend.Add(new Area(rxDef, Color.Aqua, "RX"));
    graph.Legend.Add(new Gprint(rxDef, RraType.LAST, "%6.2lf %s"));
    graph.Legend.Add(new Gprint(rxDef, RraType.AVERAGE, "%6.2lf %s"));
    graph.Legend.Add(new Gprint(rxDef, RraType.MIN, "%6.2lf %s"));
    graph.Legend.Add(new Gprint(rxDef, RraType.MAX, "%6.2lf %s\\n"));
    
    graph.Render();
}

static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
{
    var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
    return dateTime;
}