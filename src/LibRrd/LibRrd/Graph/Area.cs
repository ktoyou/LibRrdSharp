using System.Drawing;
using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph;

public class Area : IShape
{
    public Color Color { get; set; }
    
    public string Legend { get; set; }

    private readonly IValue _value;

    public Area(IValue value, Color color, string legend)
    {
        Color = color;
        Legend = legend;
        _value = value;
    }

    public override string ToString() => 
        $"AREA:{_value.Name}#{Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2")}:{Legend}";
}