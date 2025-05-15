using Microsoft.Extensions.DependencyInjection;

namespace MedicalSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add business services, validators, mediators, etc.
        // Example: services.AddScoped<IMyService, MyService>();

        return services;
    }
}