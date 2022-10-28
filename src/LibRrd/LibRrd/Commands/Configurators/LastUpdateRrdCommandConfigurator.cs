using LibRrd.DataSources;

namespace LibRrd.Commands.Configurators;

public class LastUpdateRrdCommandConfigurator : ICommandConfigurator
{
    private readonly RRD _rrd;

    public LastUpdateRrdCommandConfigurator(RRD rrd)
    {
        _rrd = rrd;
    }

    public string Configure() => $"lastupdate {_rrd.FileName}";
}