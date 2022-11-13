namespace LibRrd.Commands.Configurators;

public class RestoreRrdCommandConfigurator : ICommandConfigurator
{
    private readonly string _xmlFileName;

    private readonly string _rrdFileName;

    private readonly bool _rangeCheck;
    
    public RestoreRrdCommandConfigurator(string xmlFilename, string rrdFileName, bool rangeCheck = true)
    {
        _xmlFileName = xmlFilename;
        _rrdFileName = rrdFileName;
        _rangeCheck = rangeCheck;
    }
    
    public string Configure()
    {
        var command = $"restore {_xmlFileName} {_rrdFileName} ";
        command += _rangeCheck ? "-r" : string.Empty;
        return command;
    }
}