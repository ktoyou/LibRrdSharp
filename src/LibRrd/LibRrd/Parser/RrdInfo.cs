namespace LibRrd.Parser;

public static class RrdInfo
{
    public static T ParseRrdInfo<T>(IParser<T> parser)
    {
        return parser.Parse();
    }
}