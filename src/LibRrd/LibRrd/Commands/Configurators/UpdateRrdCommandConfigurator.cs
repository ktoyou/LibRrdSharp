namespace LibRrd.Commands.Configurators;

public class UpdateRrdCommandConfigurator : ICommandConfigurator
{
    private IEnumerable<double> _values;

    private DateTime _dateTime;

    private string _filename;

    public UpdateRrdCommandConfigurator(string filename, IEnumerable<double> values, DateTime dateTime)
    {
        _values = values;
        _dateTime = dateTime;
        _filename = filename;
    }
    
    public string Configure()
    {
        var offset = new DateTimeOffset(_dateTime);
        var command = $"update {_filename} {offset.ToUnixTimeSeconds()}";
        foreach (var val in _values) command += $":{val.ToString().Replace(',', '.')}";
        return command;
    }
}