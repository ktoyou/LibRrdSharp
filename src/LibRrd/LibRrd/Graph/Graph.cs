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
}