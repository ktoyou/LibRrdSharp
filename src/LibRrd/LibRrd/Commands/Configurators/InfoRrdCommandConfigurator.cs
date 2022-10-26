namespace LibRrd.Commands.Configurators;

public class InfoRrdCommandConfigurator : ICommandConfigurator
{
    private string _filename;

    public InfoRrdCommandConfigurator(string filename)
    {
        _filename = filename;
    }

    public string Configure() => $"info {_filename}";
}