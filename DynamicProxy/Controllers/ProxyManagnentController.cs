using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;

namespace DynamicProxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProxyManagnentController : ControllerBase
    {
        private readonly ILogger<ProxyManagnentController> _logger;
        private readonly IProxyEntries proxyEntries;
        private readonly InMemoryConfigProvider inMemoryConfigProvider;

        public ProxyManagnentController(ILogger<ProxyManagnentController> logger, IProxyEntries proxyEntries, InMemoryConfigProvider inMemoryConfigProvider)
        {
            _logger = logger;
            this.proxyEntries = proxyEntries;
            this.inMemoryConfigProvider = inMemoryConfigProvider;
        }

        [HttpGet]
        public IActionResult Get(string key)
        {
            return Ok(proxyEntries.GetAll());
        }

        [HttpPost]
        [HttpGet]
        public IResult Register(string endpointName, string forwardTo)
        {
            proxyEntries.Register(endpointName, forwardTo);
            inMemoryConfigProvider.Update(proxyEntries.GetAll().Select(s => s.Item1).ToList(), proxyEntries.GetAll().Select(s => s.Item2).ToList());
            return Results.Ok();
        }
    }
}
