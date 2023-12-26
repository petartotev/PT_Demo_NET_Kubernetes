using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DemoNetKubernetes.HealthCheck;

public class DatabaseHealthCheckService : IHealthCheck
{
    private readonly string _connectionString;

    public DatabaseHealthCheckService()
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

            return HealthCheckResult.Healthy("Database is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection failure!", ex);
        }
    }
}
