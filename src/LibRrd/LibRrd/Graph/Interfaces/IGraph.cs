namespace LibRrd.Graph.Interfaces;

public interface IGraph
{
    public int Width { get; set; }

    public int Height { get; set; }

    public string Title { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}