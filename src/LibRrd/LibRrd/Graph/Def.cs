using LibRrd.Archive;
using LibRrd.DataSources;
using LibRrd.Graph.Interfaces;

namespace LibRrd.Graph;

public class Def : IValue
{
    private readonly IDataSource _dataSource;

    private readonly RRD _rrd;

    private readonly RraType _rraType;

    private readonly string _name;
    
    public string Name => _name;

    public Def(RRD rrd, IDataSource ds, RraType rraType, string name)
    {
        _rrd = rrd;
        _dataSource = ds;
        _rraType = rraType;
        _name = name;
    }

    public override string ToString() => $"DEF:{_name}={_rrd.FileName}:{_dataSource.GetDsName()}:{_rraType}";
}