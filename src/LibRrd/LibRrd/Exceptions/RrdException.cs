namespace LibRrd.Exceptions;

public class RrdException : Exception
{
    public override string Message { get; }

    public RrdException(string message)
    {
        Message = message;
    }
}