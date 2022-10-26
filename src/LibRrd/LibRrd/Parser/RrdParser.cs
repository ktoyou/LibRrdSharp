using LibRrd.Archive;
using LibRrd.DataSources;

namespace LibRrd.Parser;

internal class RrdParser : IRrdInfoParser
{
    private string _rrdContent;
    
    public RrdParser(string rrdContent)
    {
        _rrdContent = rrdContent;
    }
    
    public RRD Parse()
    {
        var dataSources = new List<IDataSource>();
        var rraArchives = new List<IRraArchive>();
        
        var splitedRrdInfo = _rrdContent.Split("\n");

        var filename = ParseFileName(splitedRrdInfo[0]);
        var step = StepParser(splitedRrdInfo[2]);

        for (var i = 5; i < splitedRrdInfo.Length; i++)
        {
            var line = splitedRrdInfo[i];
            if (line.StartsWith("ds") && line.Contains("index"))
            {
                dataSources.Add(DataSourceParser.ParseDataSource(ref line, splitedRrdInfo));
            } else if (line.StartsWith("rra") && line.Contains("cf")) 
            {
                rraArchives.Add(RraParser.ParseRra(ref line, splitedRrdInfo));
            }
        }

        return new RRD(filename, step, dataSources, rraArchives);
    }

    private static int StepParser(string line)
    {
        var left = line.LastIndexOf(' ') + 1;
        return int.Parse(line.Substring(left));
    }

    private static string ParseFileName(string line)
    {
        var left = line.IndexOf("\"", StringComparison.Ordinal) + 1;
        var right = line.LastIndexOf("\"", StringComparison.Ordinal);
        return line.Substring(left, right - left);
    }
}