using LibRrd.Archive;
using LibRrd.DataSources;
using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph;

public class Gprint : ITextLegend
{
    private readonly Def _def;

    private readonly RraType _type;

    private readonly string _format;
    
    public Gprint(Def Def, RraType type, string format)
    {
        _def = Def;
        _type = type;
        _format = format;
    }

    public override string ToString() => $"GPRINT:{_def.Name}:{_type.ToString()}:{_format}";
}