using LibRrd.Archive;
using LibRrd.Commands;
using LibRrd.Commands.Configurators;
using LibRrd.DataSources;
using LibRrd.Parser;

namespace LibRrd;

/// <summary>
/// Главный класс для работы с RRD базами данных. Внимание: перед работой укажите в RRD_PATH путь к исполняемому файлу rrdtool
/// </summary>
public class RRD
{
    #region Fields
    
    /// <summary>
    /// Путь до исполняемого файла rrdtool
    /// </summary>
    public static string RRD_PATH = "rrdtool.exe";

    /// <summary>
    /// Источники данных (DS)
    /// </summary>
    public IEnumerable<IDataSource> DataSources { get; }

    /// <summary>
    /// Архивы данных (RRA)
    /// </summary>
    public IEnumerable<IRraArchive> RraArchives { get; }

    /// <summary>
    /// Название файла базы данных
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Шаг между данными в секундах
    /// </summary>
    public int Step { get; }

    #endregion
    
    public RRD(string filename, int step, IEnumerable<IDataSource> dataSources, IEnumerable<IRraArchive> rraArchives)
    {
        FileName = filename;
        Step = step;
        DataSources = dataSources;
        RraArchives = rraArchives;
    }


    #region Sync Methods

    /// <summary>
    /// Дамп базы данных.
    /// </summary>
    /// <returns>Строку с дампом.</returns>
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
    /// Получение последних данных об обновлении базы. Возвращает время последнего обновления. Для получения значений,
    /// нужно обратиться к полю LastValue класса унаследованного от IDataSource 
    /// </summary>
    /// <returns></returns>
    public DateTime LastUpdate()
    {
        var command = new CommandExecutor().ExecuteCommand(RRD_PATH, new LastUpdateRrdCommandConfigurator(this));
        return RrdInfo.ParseRrdInfo(new RrdLastUpdateParser(command.Output, (List<IDataSource>)DataSources));
    }

    /// <summary>
    /// Восстанавливает базу данных из файла XML
    /// </summary>
    /// <param name="xmlFilename">Путь до XML файла</param>
    /// <param name="rrdFileName">Путь для сохранения восстановленной базы</param>
    /// <param name="rangeCheck">Убедитесь, что значения в RRA не превышают пределы, определенные для различных источников данных</param>
    public static void RestoreFromXml(string xmlFilename, string rrdFileName, bool rangeCheck = true) =>
        new CommandExecutor().ExecuteCommand(RRD_PATH, new RestoreRrdCommandConfigurator(xmlFilename, rrdFileName, rangeCheck));

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

    #endregion

    #region Async Methods

    /// <summary>
    /// Получение последних данных об обновлении базы. Возвращает время последнего обновления. Для получения значений,
    /// нужно обратиться к полю LastValue класса унаследованного от IDataSource 
    /// </summary>
    /// <returns></returns>
    public async Task<DateTime> LastUpdateAsync() => await Task.Run(() => LastUpdate());
    
    /// <summary>
    /// Создание базы данных.
    /// </summary>
    /// <param name="filename">Путь для сохранения базы данных.</param>
    /// <param name="step">Шаг.</param>
    /// <param name="dataSources">Источники данных (DS).</param>
    /// <param name="rraArchives">Архивы данных (RRA).</param>
    /// <returns>База данных RRD</returns>
    public static async Task<RRD> CreateAsync(string filename, int step, IEnumerable<IDataSource> dataSources,
        IEnumerable<IRraArchive> rraArchives) => await Task.Run(() => Create(filename, step, dataSources, rraArchives));

    /// <summary>
    /// Возвращает объект базы данных RRD для последующей работы с ней.
    /// </summary>
    /// <param name="filename">Путь до базы данных.</param>
    /// <returns></returns>
    public static async Task<RRD> LoadAsync(string filename) => await Task.Run(() => Load(filename));

    /// <summary>
    /// Дамп базы данных.
    /// </summary>
    /// <returns>Строку с дампом.</returns>
    public async Task<string?> DumpAsync() => await Task.Run(() => Dump());

    /// <summary>
    /// Обновление источников данных (DS). Обновлять значения в том порядке, в каком идут источники данных.
    /// </summary>
    /// <param name="values">Значения для источников данных (DS).</param>
    /// <param name="dateTime">Время обновления.</param>
    public async Task UpdateAsync(IEnumerable<double> values, DateTime dateTime) => await Task.Run(() => Update(values, dateTime));

    /// <summary>
    /// Восстанавливает базу данных из файла XML
    /// </summary>
    /// <param name="xmlFilename">Путь до XML файла</param>
    /// <param name="rrdFileName">Путь для сохранения восстановленной базы</param>
    /// <param name="rangeCheck">Убедитесь, что значения в RRA не превышают пределы, определенные для различных источников данных</param>
    public async Task RestoreFromXmlAsync(string xmlFilename, string rrdFileName, bool rangeCheck = true) =>
        await Task.Run(() => RestoreFromXml(xmlFilename, rrdFileName, rangeCheck));

    #endregion
}