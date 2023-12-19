using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace DemoNetKubernetes.Controllers;

[ApiController]
[Route("[controller]")]
public class MemoryController : ControllerBase
{
    private readonly ILogger<MemoryController> _logger;

    public MemoryController(ILogger<MemoryController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "DoLoadOnMemory")]
    [Route("doload/{megabytes}")]
    public ObjectResult Get(int megabytes)
    {
        ConsumeMemory(megabytes);

        var counter = Metrics.CreateCounter(
            "memory_endpoint_requests_count",
            "memory_endpoint_requests_count gets incremented on requests upon Memory endpoints.");
        counter.Inc();

        return Ok("Memory intensive process completed!");
    }

    private static void ConsumeMemory(int megabytes = 10)
    {
        byte[] largeArray = new byte[megabytes * 1024 * 1024]; // Allocate a large array

        // Fill the array to consume memory
        for (int i = 0; i < largeArray.Length; i++)
        {
            largeArray[i] = 1;
        }
    }
}
