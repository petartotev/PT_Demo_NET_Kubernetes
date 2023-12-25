using DemoNetKubernetes.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace DemoNetKubernetes.Controllers;

// https://redis.io/docs/connect/clients/dotnet/
[ApiController]
[Route("[controller]")]
public class RedisController : ControllerBase
{
    private readonly ILogger<RedisController> _logger;
    private readonly IDatabase _database;

    public RedisController(ILogger<RedisController> logger)
    {
        _logger = logger;

        _logger.LogInformation($"ConnectionMultiplexer.Connect(\"redis-master:6379\") about to be executed with password!");

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-master:6379,password=usOZHSp51Z");
        _database = redis.GetDatabase();

        _logger.LogInformation($"ConnectionMultiplexer.Connect(\"redis-master:6379\") successful!");
    }

    [HttpPost(Name = "SetRedisValueByKey")]
    [Route("set")]
    public ObjectResult Set(RedisObject obj)
    {
        if (_database.StringSet(obj.Key, obj.Value)) return Ok($"{obj.Key}:{obj.Value} stored in Redis!");

        return BadRequest($"Redis did not manage to store {obj.Key}:{obj.Value}!");
    }

    [HttpGet(Name = "GetRedisKeyByValue")]
    [Route("get/{key}")]
    public async Task<ObjectResult> GetAsync(string key)
    {
        var value = await _database.StringGetAsync(key);

        if (!value.IsNull && value.HasValue) return Ok(value.ToString());

        return NotFound($"Redis value was not found by key {key}!");
    }
}
