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
    private readonly IDatabase _databaseMaster;
    private readonly IDatabase _databaseReadReplicas;

    public RedisController(ILogger<RedisController> logger)
    {
        _logger = logger;

        _logger.LogInformation($"ConnectionMultiplexer.Connect(\"redis-master:6379\") about to be executed with password!");
        ConnectionMultiplexer redisMaster = ConnectionMultiplexer.Connect("redis-master:6379,password=6t5eH7touz");
        _databaseMaster = redisMaster.GetDatabase();
        _logger.LogInformation($"ConnectionMultiplexer.Connect(\"redis-master:6379\") successful!");

        _logger.LogInformation($"ConnectionMultiplexer.Connect(\"redis-replicas:6379\") about to be executed with password!");
        ConnectionMultiplexer redisReadReplicas = ConnectionMultiplexer.Connect("redis-replicas:6379,password=6t5eH7touz");
        _databaseReadReplicas = redisReadReplicas.GetDatabase();
        _logger.LogInformation($"ConnectionMultiplexer.Connect(\"redis-replicas:6379\") successful!");
    }

    [HttpPost(Name = "SetRedisValueByKey")]
    [Route("set")]
    public ObjectResult Set(RedisEntity obj)
    {
        if (_databaseMaster.StringSet(obj.Key, obj.Value)) return Ok($"{obj.Key}:{obj.Value} stored in Redis!");

        return BadRequest($"Redis failed to store {obj.Key}:{obj.Value}!");
    }

    [HttpGet(Name = "GetRedisValueByKey")]
    [Route("get/{key}")]
    public async Task<ObjectResult> GetAsync(string key)
    {
        var value = await _databaseReadReplicas.StringGetAsync(key);

        if (!value.IsNull && value.HasValue) return Ok(value.ToString());

        return NotFound($"Redis key `{key}` not found!");
    }

    [HttpDelete(Name = "DeleteRedisKey")]
    [Route("delete/{key}")]
    public async Task<ObjectResult> DeleteAsync(string key)
    {
        var isDeleted = await _databaseMaster.KeyDeleteAsync(key);

        return isDeleted ? Ok($"Redis key `{key}` deleted!") : NotFound($"Redis key `{key}` not found!");
    }
}
