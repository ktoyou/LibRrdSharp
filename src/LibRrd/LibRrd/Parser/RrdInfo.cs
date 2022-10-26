namespace LibRrd.Parser;

public static class RrdInfo
{
    public static RRD ParseRrdInfo(IRrdInfoParser parser)
    {
        return parser.Parse();
    }
}