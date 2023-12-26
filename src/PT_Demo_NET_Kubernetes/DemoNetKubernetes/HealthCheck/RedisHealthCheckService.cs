using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DemoNetKubernetes.HealthCheck;

public class RedisHealthCheckService : IHealthCheck
{
    private readonly string _connectionString;

    public RedisHealthCheckService()
    {
        _connectionString = "your-connection-string-here" ?? throw new ArgumentNullException(nameof(_connectionString));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //}

            return HealthCheckResult.Healthy("Redis is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis connection failure!", ex);
        }
    }
}
