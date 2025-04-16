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
            return Registrations.DistinctBy(x => x.Key).Select((x,i) => (new RouteConfig
            {
                RouteId = new Guid().ToString(),
                ClusterId = i.ToString(),
                Match = new RouteMatch
                {
                    Path = $"{x.Key}/{{**catch-all}}"
                },
                Transforms = CreatePathTransform(x)
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

        private IReadOnlyList<IReadOnlyDictionary<string, string>>? CreatePathTransform(KeyValuePair<string, string> keyValuePair)
        {
            Dictionary<string, string> path = new Dictionary<string, string>();
            path.Add("PathRemovePrefix", $"{keyValuePair.Key}");
            return [path.AsReadOnly()];
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
