using LibRrd.Archive;

namespace LibRrd.Parser;

public static class RraParser
{
    public static RRA ParseRra(ref string line, string[] splitedRrdInfo)
    {
        var index = ParseIndex(line);
        var rows = 0;
        double xff = 0;
        var rraType = RraType.LAST;

        for (var i = Array.IndexOf(splitedRrdInfo, line); line.Contains($"rra[{index}]"); i++)
        {
            if (line.Contains("cf")) rraType = ParseType(line);
            else if (line.Contains("rows")) rows = int.Parse(ParseLine(line));
            else if (line.Contains("xff")) xff = double.Parse(ParseLine(line));
            
            line = splitedRrdInfo[i];
        }

        return new RRA(rraType, rows, xff);
    }

    private static string ParseLine(string line)
    {
        var left = line.LastIndexOf(' ') + 1;
        return line.Substring(left);
    }
    
    private static int ParseIndex(string line)
    {
        var leftSeparator = line.IndexOf('[') + 1;
        var rightSeparator = line.IndexOf(']');

        return int.Parse(line.Substring(leftSeparator, rightSeparator - leftSeparator));
    }

    private static RraType ParseType(string line)
    {
        var left = line.IndexOf("\"", StringComparison.Ordinal) + 1;
        var right = line.LastIndexOf("\"", StringComparison.Ordinal);
        return (RraType)Enum.Parse(typeof(RraType), line.Substring(left, right - left));
    }
}