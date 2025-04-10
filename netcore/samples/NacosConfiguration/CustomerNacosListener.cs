using Nacos.V2;

namespace NacosConfiguration;

public class CustomerNacosListener : IListener
{
    public void ReceiveConfigInfo(string configInfo)
    {
        Console.WriteLine("received:" + configInfo);
    }
}