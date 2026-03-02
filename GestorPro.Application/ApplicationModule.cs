using Microsoft.Extensions.DependencyInjection;

namespace GestorPro.Application;

public static class ApplicationModule
{
    public static void AddApplicationModule(this IServiceCollection services)
    {
        services.AddServices();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}
