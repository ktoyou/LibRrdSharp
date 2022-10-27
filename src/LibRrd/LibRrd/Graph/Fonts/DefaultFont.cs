using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph.Fonts;

public class DefaultFont : IFont
{
    public string FontName { get; set; }
    
    public int FontSize { get; set; }
    
    public DefaultFont(string fontName, int fontSize)
    {
        FontName = fontName;
        FontSize = fontSize;
    }

    public override string ToString() => $"--font DEFAULT:{FontSize}:\"{FontName}\"";
}