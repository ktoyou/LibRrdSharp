using LibRrd.Archive;
using LibRrd.Commands;
using LibRrd.Commands.Configurators;
using LibRrd.DataSources;
using LibRrd.Parser;

namespace LibRrd;

public class RRD
{
    public static string RRD_PATH = "rrdtool.exe";

    public IEnumerable<IDataSource> DataSources { get; }

    public IEnumerable<IRraArchive> RraArchives { get; }

    public string FileName { get; }

    public int Step { get; }

    public RRD(string filename, int step, IEnumerable<IDataSource> dataSources, IEnumerable<IRraArchive> rraArchives)
    {
        FileName = filename;
        Step = step;
        DataSources = dataSources;
        RraArchives = rraArchives;
    }

    public string Dump()
    {
        var command = new CommandExecutor().ExecuteCommand(RRD_PATH, new DumpRrdConfigurator(FileName));
        return command.Output;
    }

    public void Update(IEnumerable<double> values, DateTime dateTime) => new CommandExecutor().ExecuteCommand(RRD_PATH, new UpdateRrdCommandConfigurator(FileName, values, dateTime));

    public static RRD Create(string filename, int step, IEnumerable<IDataSource> dataSources, IEnumerable<IRraArchive> rraArchives)
    {
        var command = new CommandExecutor().ExecuteCommand(RRD_PATH, new CreateRrdCommandConfigurator(filename, step, dataSources, rraArchives));
        return new RRD(filename, step, dataSources, rraArchives);
    }

    public static RRD Load(string filename)
    {
        var command = new CommandExecutor().ExecuteCommand(RRD_PATH, new InfoRrdCommandConfigurator(filename));
        return RrdInfo.ParseRrdInfo(new RrdParser(command.Output));
    }
    
}