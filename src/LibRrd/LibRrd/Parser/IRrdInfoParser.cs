namespace LibRrd.Parser;

public interface IRrdInfoParser : IParser<RRD>
{ 
    RRD Parse();
}