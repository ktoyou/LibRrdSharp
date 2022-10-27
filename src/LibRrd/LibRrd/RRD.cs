using LibRrd.Archive;
using LibRrd.Commands;
using LibRrd.Commands.Configurators;
using LibRrd.DataSources;
using LibRrd.Graph;
using LibRrd.Parser;

namespace LibRrd;

/// <summary>
/// Главный класс для работы с RRD базами данных. Внимание: перед работой укажите в RRD_PATH путь к исполняемому файлу rrdtool
/// </summary>
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

    public string? Dump()
    {
        var command = new CommandExecutor().ExecuteCommand(RRD_PATH, new DumpRrdConfigurator(FileName));
        return command.Output;
    }

    /// <summary>
    /// Обновление источников данных (DS). Обновлять значения в том порядке, в каком идут источники данных.
    /// </summary>
    /// <param name="values">Значения для источников данных (DS).</param>
    /// <param name="dateTime">Время обновления.</param>
    public void Update(IEnumerable<double> values, DateTime dateTime) => new CommandExecutor().ExecuteCommand(RRD_PATH, new UpdateRrdCommandConfigurator(FileName, values, dateTime));

    
    /// <summary>
    /// Создание базы данных.
    /// </summary>
    /// <param name="filename">Путь для сохранения базы данных.</param>
    /// <param name="step">Шаг.</param>
    /// <param name="dataSources">Источники данных (DS).</param>
    /// <param name="rraArchives">Архивы данных (RRA).</param>
    /// <returns>База данных RRD</returns>
    public static RRD Create(string filename, int step, IEnumerable<IDataSource> dataSources, IEnumerable<IRraArchive> rraArchives)
    {
        var command = new CommandExecutor().ExecuteCommand(RRD_PATH, new CreateRrdCommandConfigurator(filename, step, dataSources, rraArchives));
        return new RRD(filename, step, dataSources, rraArchives);
    }

    
    /// <summary>
    /// Возвращает объект базы данных RRD для последующей работы с ней.
    /// </summary>
    /// <param name="filename">Путь до базы данных.</param>
    /// <returns></returns>
    public static RRD Load(string filename)
    {
        var command = new CommandExecutor().ExecuteCommand(RRD_PATH, new InfoRrdCommandConfigurator(filename));
        return RrdInfo.ParseRrdInfo(new RrdParser(command.Output));
    }
    
    /// <summary>
    /// Взятие источника данных (DS) по имени.
    /// </summary>
    /// <param name="name">Имя источника данных.</param>
    /// <returns>Источник данных.</returns>
    public IDataSource? GetDataSourceByName(string name) => DataSources.FirstOrDefault(ds => ds.GetDsName() == name);

    /// <summary>
    /// Взятие архива данных (RRA) по типу.
    /// </summary>
    /// <param name="rraType">Тип архива.</param>
    /// <returns>Архив.</returns>
    public IRraArchive? GetRraArchive(RraType rraType) => RraArchives.FirstOrDefault(rra => rra.RraType == rraType);
}