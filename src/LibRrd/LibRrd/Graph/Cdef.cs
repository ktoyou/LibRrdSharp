using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph;

public class Cdef : IValue
{
    public string Name { get; set; }

    public string Expression { get; set; }
    
    public Cdef(string name, string expression)
    {
        Name = name;
        Expression = expression;
    }

    public override string ToString() => $"CDEF:{Name}={Expression}";
}