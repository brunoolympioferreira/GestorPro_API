using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Services;
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
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>();
        return services;
    }
}
