using System.Drawing;
using LibRrd.Commands;
using LibRrd.Commands.Configurators;
using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph;

public class Graph : IGraph
{
    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public string Title { get; set; }

    public string File { get; set; }

    public string VerticalLabel { get; set; }
    
    public bool NoGridFit { get; set; }

    public bool XAxis { get; set; }

    public bool YAxis { get; set; }

    public bool EnabledLegend { get; set; }

    public string Watermark { get; set; }

    public ImgFormat ImgFormat { get; set; }

    public DateTime Start { get; set; }
    
    public DateTime End { get; set; }

    public List<Def> Defs { get; set; }
    
    public List<Cdef> Cdefs { get; set; }
    
    public List<ILegend> Legend { get; set; }

    public IFont? TitleFont { get; set; }

    public IFont? WatermarkFont { get; set; }

    public IFont? DefaultFont { get; set; }

    public Graph(int width, int height, DateTime start, DateTime end)
    {
        Height = height;
        Width = width;
        Start = start;
        End = end;
        YAxis = true;
        EnabledLegend = true;
        XAxis = true;
        ImgFormat = ImgFormat.Png;
        Defs = new List<Def>();
        Cdefs = new List<Cdef>();
        Legend = new List<ILegend>();
        VerticalLabel = string.Empty;
        Watermark = string.Empty;
        File = string.Empty;
        Title = string.Empty;
    }

#if _WINDOWS
    public Image GetRenderedImage()
    {
        using var stream = System.IO.File.Open(File, FileMode.Open);
        var image = Image.FromStream(stream);
        stream.Close();

        return image;
    }
#endif
    
    /// <summary>
    /// Сгенерировать график.
    /// </summary>
    public void Render() => new CommandExecutor().ExecuteCommand(RRD.RRD_PATH, new GenerateRrdGraphCommandConfigurator(this));

    public override string ToString()
    {
        var command = $"graph {File} " +
                      $"-s {((DateTimeOffset)Start).ToUnixTimeSeconds()} " +
                      $"-e {((DateTimeOffset)End).ToUnixTimeSeconds()} " +
                      $"-w {Width} -h {Height} -t \"{Title}\"";
        
        if (NoGridFit) command += " -N";
        if (!XAxis) command += " --x-grid none";
        if (!YAxis) command += " --y-grid none";
        if (!EnabledLegend) command += " --no-legend";
        if (Watermark != string.Empty) command += $" --watermark \"{Watermark}\"";

        if (DefaultFont != null) command += $" {DefaultFont}";
        if (TitleFont != null) command += $" {TitleFont}";
        if (WatermarkFont != null) command += $" {WatermarkFont}";

        command += $" --imgformat {ImgFormat.ToString().ToUpper()}";
        
        Defs.ForEach(elem => command += $" {elem}");
        Cdefs.ForEach(elem => command += $" {elem}");
        Legend.ForEach(elem => command += $" {elem}");

        return command;
    }
}