namespace LibRrd.Graph;

public class Comment
{
    private readonly string _content;
    
    public Comment(string content)
    {
        _content = content;
    }

    public override string ToString() => $"COMMENT:\"{_content}\"";
}