using LibRrd.Archive;
using LibRrd.DataSources;

namespace LibRrd.Commands.Configurators;

public class CreateRrdCommandConfigurator : ICommandConfigurator
{
    private string _filename;

    private int _step;

    private IEnumerable<IDataSource> _dataSources;

    private IEnumerable<IRraArchive> _rraArchives;

    public CreateRrdCommandConfigurator(string filename, int step, IEnumerable<IDataSource> dataSources, IEnumerable<IRraArchive> rraArchives)
    {
        _filename = filename;
        _step = step;
        _dataSources = dataSources;
        _rraArchives = rraArchives;
    }
    
    public string Configure()
    {
        var command = $"create {_filename} --step {_step}";
        foreach (var ds in _dataSources) command += $" {ds}";
        foreach (var rra in _rraArchives) command += $" {rra}";
        return command;
    }
}