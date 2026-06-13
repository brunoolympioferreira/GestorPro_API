using GestorPro.Domain.Interfaces.Contracts;
using GestorPro.Domain.Interfaces.Repositories;
using GestorPro.Infra.Persistence;
using GestorPro.Infra.Persistence.Interceptors;
using GestorPro.Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestorPro.Infra;

public static class InfraModule
{
    public static void AddInfraModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DatabaseConnection") ?? throw new ArgumentNullException("ConnectionString Requerida");
        services
            .AddDatabase(connectionString)
            .AddUnityOfWork()
            .AddRepositories()
            .AddInterceptors();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString)
                   .AddInterceptors(sp.GetRequiredService<FixEntityStateInterceptor>());
        });
        return services;
    }

    private static IServiceCollection AddUnityOfWork(this IServiceCollection services)
    {
        return services.AddScoped<IUnityOfWork, UnityOfWork>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IUnitOfMeasureRepository, UnitOfMeasureRepository>()
            .AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

        return services;
    }

    private static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services
            .AddScoped<FixEntityStateInterceptor>();
        return services;
    }
}
