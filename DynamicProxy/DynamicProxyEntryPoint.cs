using Yarp.ReverseProxy.Configuration;

namespace DynamicProxy
{
    public class DynamicProxyEntryPoint
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<IProxyEntries>(new ProxyRegistrations());
            builder.Services.AddReverseProxy().LoadFromMemory(Routes(), Clusters());
            builder.Services.AddControllers();
            var app = builder.Build();
            app.MapControllers();
            app.MapReverseProxy();
            app.Run();
        }

        private static IReadOnlyList<ClusterConfig> Clusters()
        {
            return new List<ClusterConfig>();
        }

        private static IReadOnlyList<RouteConfig> Routes()
        {
            return new List<RouteConfig>();
        }
    }
}
