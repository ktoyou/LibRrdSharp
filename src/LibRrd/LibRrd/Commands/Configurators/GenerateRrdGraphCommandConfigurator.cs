namespace LibRrd.Commands.Configurators;

public class GenerateRrdGraphCommandConfigurator : ICommandConfigurator
{
    private readonly Graph.Graph _graph;
    
    public GenerateRrdGraphCommandConfigurator(Graph.Graph graph)
    {
        _graph = graph;
    }
    
    public string Configure()
    {
        var command = $"graph {_graph.File} " +
                      $"-s {((DateTimeOffset)_graph.Start).ToUnixTimeSeconds()} " +
                      $"-e {((DateTimeOffset)_graph.End).ToUnixTimeSeconds()} " +
                      $"-w {_graph.Width} -h {_graph.Height} -t \"{_graph.Title}\"";
        
        if (_graph.NoGridFit) command += " -N";
        if (!_graph.XAxis) command += " --x-grid none";
        if (!_graph.YAxis) command += " --y-grid none";
        if (!_graph.Legend) command += " --no-legend";
        if (_graph.Watermark != string.Empty) command += $" --watermark \"{_graph.Watermark}\"";

        if (_graph.DefaultFont != null) command += $" {_graph.DefaultFont}";
        if (_graph.TitleFont != null) command += $" {_graph.TitleFont}";
        if (_graph.WatermarkFont != null) command += $" {_graph.WatermarkFont}";

        command += $" --imgformat {_graph.ImgFormat.ToString().ToUpper()}";
        
        _graph.Comments.ForEach(elem => command += $" {elem}");
        _graph.Defs.ForEach(elem => command += $" {elem}");
        _graph.Cdefs.ForEach(elem => command += $" {elem}");
        _graph.Shapes.ForEach(elem => command += $" {elem}");
        _graph.Gprints.ForEach(elem => command += $" \"{elem}\"");

        return command;
    }
}