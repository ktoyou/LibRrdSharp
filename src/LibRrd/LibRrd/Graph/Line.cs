using System.Drawing;
using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph;

public class Line : IShape
{
    public Color Color { get; set; }
    
    public string Legend { get; set; }

    public int Thickness { get; set; }
    
    private readonly IValue _value;

    public Line(IValue value, Color color, int thickness, string legend)
    {
        Color = color;
        Legend = legend;
        Thickness = thickness;
        _value = value;
    }

    public override string ToString() => $"LINE{Thickness}:{_value.Name}#{Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2")}:{Legend}";
}