namespace LibRrd.DataSources;

public class DS : IDataSource
{
    private readonly string _name;

    private readonly DataType _dataType;

    private readonly int _heartbeat;

    private readonly float _min;

    private readonly float _max;

    public string Name => _name;

    public DataType DataType => _dataType;

    public int HeartBeat => _heartbeat;

    public float Min => _min;

    public float Max => _max;

    public float? LastValue { get; set; }
    
    public DS(string name, DataType type, int heartbeat, float min, float max)
    {
        if (heartbeat < 0) throw new Exception("Heartbeat can't less than 0");
        
        _name = name;
        _dataType = type;
        _heartbeat = heartbeat;
        _min = min;
        _max = max;
    }
    
    public string GetDsName() => _name;

    public override string ToString() => $"DS:{_name}:{_dataType.ToString().ToUpper()}:{_heartbeat}:{_min.ToString().Replace(',', '.')}:{_max.ToString().Replace(',', '.')}";
}