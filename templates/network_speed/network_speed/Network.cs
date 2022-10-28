using System.Net.NetworkInformation;

namespace network_speed;

public class Network
{
    private readonly NetworkInterface[] _networkInterfaces;
    
    public Network()
    {
        _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
    }

    public async Task<long> GetCurrentInterfaceSpeedAsync(string interfaceName, int interval)
    {
        var @interface = _networkInterfaces.FirstOrDefault(elem => elem.Name == interfaceName);
        var received = @interface.GetIPStatistics().BytesReceived;
        await Task.Delay(interval);
        var newReceived = @interface.GetIPStatistics().BytesReceived;
        return newReceived - received;
    }
}