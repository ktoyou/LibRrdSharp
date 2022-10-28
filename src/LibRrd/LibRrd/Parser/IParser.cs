namespace LibRrd.Parser;

public interface IParser<T>
{
    T Parse();
}