using LibRrd.DataSources;

namespace LibRrd.Parser;

public class RrdLastUpdateParser : IParser<DateTime>
{
    private readonly List<IDataSource> _datasources;

    private readonly string _input;

    public RrdLastUpdateParser(string input, List<IDataSource> dataSources)
    {
        _datasources = dataSources;
        _input = input;
    }
    
    public DateTime Parse()
    {
        var splitedInput = _input.Replace('.', ',').Substring(_input.IndexOf("\n\n") + 2).Split(" ");
        splitedInput[0] = splitedInput[0].Replace(':', ' ');
        splitedInput[splitedInput.Length - 1] = splitedInput[splitedInput.Length - 1].Replace('\n', ' ');

        var lastUpdateTime = DateTimeOffset.FromUnixTimeSeconds(int.Parse(splitedInput[0]));

        for (var i = 0; i < splitedInput.Length - 1; i++)
        {
            if(splitedInput[i + 1].Contains('U')) continue;
            _datasources[i].LastValue = float.Parse(splitedInput[i + 1]);
        }

        return lastUpdateTime.Date;
    }
}