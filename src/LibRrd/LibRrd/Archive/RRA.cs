namespace LibRrd.Archive;

public class RRA : IRraArchive
{
    private readonly RraType _type;

    private readonly int _length;

    private readonly double _xff;
    
    public RRA(RraType type, int length, double xff)
    {
        if (xff < 0 || xff > 1) throw new Exception("xff must be in the range from 0 to 1");
        
        _type = type;
        _length = length;
        _xff = xff;
    }

    public override string ToString() => $"RRA:{_type.ToString().ToUpper()}:{_xff.ToString().Replace(',', '.')}:1:{_length}";
}