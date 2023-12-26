using DemoNetKubernetes.HealthCheck;
using Prometheus;

namespace DemoNetKubernetes;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.UseHttpClientMetrics();

        builder.Services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheckService>("database_health_check")
            .AddCheck<RedisHealthCheckService>("redis_health_check");

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger().UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapMetrics(); // Exposes /metrics endpoint
            endpoints.MapControllers();
        });

        app.Run();
    }
}