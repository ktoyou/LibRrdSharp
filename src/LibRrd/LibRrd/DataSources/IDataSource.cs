namespace LibRrd.DataSources;

public interface IDataSource
{
    public float? LastValue { get; set; }
    
    string GetDsName();
}