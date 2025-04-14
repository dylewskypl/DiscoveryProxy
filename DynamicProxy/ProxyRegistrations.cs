using System.Collections.Specialized;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace DynamicProxy
{
    internal class ProxyRegistrations : IProxyEntries
    {
        private static Dictionary<string, string> Registrations { get; } = new Dictionary<string, string>();

        public string Get(string key)
        {
            return Registrations[key];
        }

        public List<(RouteConfig,ClusterConfig)> GetAll()
        {
            return Registrations.Select((x,i)=> (new RouteConfig
            {
                RouteId = new Guid().ToString(),
                ClusterId = i.ToString(),
                Match = new RouteMatch
                {
                    Path = $"{x.Key}/{{**catch-all}}"
                }
            },
            new ClusterConfig
            {
                ClusterId = i.ToString(),
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { i.ToString(), new DestinationConfig
                        {
                            Address = $"http://localhost:{x.Value}"
                        }
                    }
                }
            })).ToList();
        }

        public bool Register(string key, string value)
        {
            Registrations[key] = value;
            return true;
        }

        public bool Unregister(string key)
        {
            Registrations.Remove(key);
            return true;
        }
    }
}