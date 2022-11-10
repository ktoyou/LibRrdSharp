namespace LibRrd.Commands.Configurators;

public class GenerateRrdGraphCommandConfigurator : ICommandConfigurator
{
    private readonly Graph.Graph _graph;
    
    public GenerateRrdGraphCommandConfigurator(Graph.Graph graph)
    {
        _graph = graph;
    }

    public string Configure() => _graph.ToString();
}