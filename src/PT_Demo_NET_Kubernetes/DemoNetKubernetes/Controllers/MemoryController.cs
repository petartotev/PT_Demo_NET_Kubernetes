using Microsoft.AspNetCore.Mvc;

namespace DemoNetKubernetes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemoryController : ControllerBase
    {
        private readonly ILogger<CpuController> _logger;

        public MemoryController(ILogger<CpuController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "DoLoadOnMemory")]
        [Route("doload/{megabytes}")]
        public ObjectResult Get(int megabytes)
        {
            ConsumeMemory(megabytes);

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
}
