using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph.Fonts;

public class WatermarkFont : IFont
{
    public string FontName { get; set; }
    
    public int FontSize { get; set; }
    
    public WatermarkFont(string fontName, int fontSize)
    {
        FontName = fontName;
        FontSize = fontSize;
    }

    public override string ToString() => $"--font WATERMARK:{FontSize}:\"{FontName}\"";
}