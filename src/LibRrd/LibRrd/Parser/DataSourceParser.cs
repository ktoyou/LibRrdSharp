using LibRrd.DataSources;

namespace LibRrd.Parser;

public class DataSourceParser
{
    public static DS ParseDataSource(ref string line, string[] splitedRrdInfo)
    {
        var dsName = ParseDataSourceName(line);
        var heartbeat = 0;
        float min = 0, max = 0;
        var dataType = DataType.GAUGE;
        
        for (var j = Array.IndexOf(splitedRrdInfo, line); line.Contains($"ds[{dsName}]"); j++)
        {
            line = splitedRrdInfo[j];
            if (line.Contains("type")) dataType = ParseDataType(line);
            else if (line.Contains("minimal_heartbeat")) heartbeat = int.Parse(ParseLine(line));
            else if (line.Contains("min")) min = float.Parse(ParseLine(line));
            else if (line.Contains("max")) max = float.Parse(ParseLine(line));
        }
        
        return new DS(dsName, dataType, heartbeat, min, max);
    }

    private static string ParseDataSourceName(string line)
    {
        var leftSeparator = line.IndexOf('[') + 1;
        var rightSeparator = line.IndexOf(']');
        return line.Substring(leftSeparator, rightSeparator - leftSeparator);
    }
    
    private static string ParseLine(string line)
    {
        var left = line.LastIndexOf(' ') + 1;
        return line.Substring(left);
    }

    private static DataType ParseDataType(string line)
    {
        var left = line.IndexOf("\"", StringComparison.Ordinal) + 1;
        var right = line.LastIndexOf("\"", StringComparison.Ordinal);
        return (DataType)Enum.Parse(typeof(DataType), line.Substring(left, right - left));
    }
}