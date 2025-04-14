using Yarp.ReverseProxy.Configuration;

namespace DynamicProxy
{
    public interface IProxyEntries
    {
        bool Register(string key, string value);
        bool Unregister(string key);
        string Get(string key);
        List<(RouteConfig, ClusterConfig)> GetAll();
    }
}