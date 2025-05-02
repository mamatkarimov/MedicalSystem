using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.Health;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AuthService.Infrastructure.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddInfrastructureHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(
                    name: "database",
                    tags: new[] { "infrastructure" })
                .AddCheck<RedisHealthCheck>(
                    "redis",
                    HealthStatus.Degraded,
                    new[] { "infrastructure", "cache" });

            return services;
        }

        public static IApplicationBuilder UseInfrastructureHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(HealthReportSerializer.Serialize(report));
                }
            });

            return app;
        }
    }
}