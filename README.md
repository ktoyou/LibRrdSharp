# LibRrdSharp
## Обертка для удобной работы с rrdtool посредством C#

Внимание! В данный момент не все функции доступны. 
LibRrdSharp может проделывать основные вещи с базами rrdtool и графиками.

## Фишки
- Генерирование графиков
- Создание баз данных
- Загрузка баз данных
- Дампинг баз данных в xml формат
- Обновление баз данных 

### Installation

Перед тем, как использовать библиотеку, нужно произвести сборку.

Для Linux..
```sh
cd LibRrd/src
dotnet build
```

Для Windows нужно открыть Visual Studio 2019-2022 и нажать на кнопку build для сборки.
После проделанных манипуляций библиотека готова к использованию.


## Пример использования

Перед использованием, нужно создать базу данных либо загрузить её.
Для это предусмотрены 2 метода в классе `RRD` (`Create`, `Load`) соответственно.

> Перед всеми действиями, скачайте или соберите rrdtool под вашу ОС, 
> и укажите полный путь к исполняемому файлу rrdtool в статической переменной `RRD_PATH`.
> В противном случае будет исключение.


### Создание базы

```Csharp 
var rrd = RRD.Create("system.rrd", step, new List<IDataSource>()
{
    new DS("template", DataType.GAUGE, 20, 0, 100)
}, new List<IRraArchive>()
{
    new RRA(RraType.LAST, 100, 0.5),
    new RRA(RraType.AVERAGE, 100, 0.5),
    new RRA(RraType.MIN, 100, 0.5),
    new RRA(RraType.MAX, 100, 0.5)
});
```

### Загрузка базы

```Csharp 
var rrd = RRD.Load("test.rrd");
```

### Дамп базы

```Csharp 
var xml = rrd.Dump(); // Возвращает строку 
```

### Создание графика

> После вызова метода `Render`, график сохрантися по пути который вы указали в `FileName`.
> Если нужно получить объект класса `Image`, для этого предназначен метод `GetRenderedImage`.

```Csharp 
var graph = new Graph(800, 360, DateTime.Now, DateTime.Now)
{
    Title = "Test",
    File = "test.png",
    Watermark = "Test",
    TitleFont = new TitleFont("Ubuntu Mono Medium", 15),
    WatermarkFont = new WatermarkFont("Ubuntu Mono Medium", 10),
    DefaultFont = new DefaultFont("Ubuntu Mono Medium", 10)
};

graph.Defs.Add(new Def(rrd, rrd.GetDataSourceByName("template"), RraType.LAST, "template"));
graph.Cdefs.Add(new Cdef("memory_cdef", $"template_def,UN,0,template_def,IF"));
    
graph.Legend.Add(new Comment("Cur".PadLeft(23)));
graph.Legend.Add(new Comment("Avg".PadLeft(9)));
graph.Legend.Add(new Comment("Min".PadLeft(8)));
graph.Legend.Add(new Comment("Max\\n".PadLeft(9)));
    
graph.Legend.Add(new Line(graph.Defs.First(elem => elem.Name == "template_def"), Color.Brown, 1, "TestValue"));
graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "template_def"), RraType.LAST, "%6.2lf %%".PadLeft(15)));
graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "template_def"), RraType.AVERAGE, "%6.2lf %%"));
graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "template_def"), RraType.MIN, "%6.2lf %%"));
graph.Legend.Add(new Gprint(graph.Defs.First(elem => elem.Name == "template_def"), RraType.MAX, "%6.2lf %%\\n"));

graph.Render();
```

## License

MIT
