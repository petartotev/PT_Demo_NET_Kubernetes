using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DemoNetKubernetes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnvController : ControllerBase
    {
        [HttpGet(Name = "GetAllEnvVarsFromKubernetes")]
        [Route("getenvvars")]
        public ObjectResult Get()
        {
            string base64EncodedPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

            string appConfig = Environment.GetEnvironmentVariable("APP_CONFIG");

            var responseMessage = new StringBuilder()
                .AppendLine($"DATABASE_PASSWORD: {base64EncodedPassword}")
                .AppendLine($"APP_CONFIG: {appConfig}")
                .ToString();

            return Ok(responseMessage);
        }
    }
}
