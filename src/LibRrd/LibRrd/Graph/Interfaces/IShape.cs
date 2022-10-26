using System.Drawing;

namespace LibRrd.Graph.Interfaces;

public interface IShape
{
    public Color Color { get; set; }

    public string Legend { get; set; }
}