namespace LibRrd.Commands.Configurators;

public class DumpRrdConfigurator : ICommandConfigurator
{
    private string _filename;
    
    public DumpRrdConfigurator(string filename)
    {
        _filename = filename;
    }

    public string Configure() => $"dump {_filename}";
}