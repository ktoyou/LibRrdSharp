using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph.Fonts;

public class TitleFont : IFont
{
    public string FontName { get; set; }
    
    public int FontSize { get; set; }

    public TitleFont(string fontName, int fontSize)
    {
        FontName = fontName;
        FontSize = fontSize;
    }

    public override string ToString() => $"--font TITLE:{FontSize}:\"{FontName}\"";
}