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
            .AddRepositories();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        //services.AddDbContext<MecGestorDbContext>(options =>
        //{
        //    options.UseSqlServer(connectionString);
        //});
        return services;
    }

    private static IServiceCollection AddUnityOfWork(this IServiceCollection services)
    {
        //return services.AddScoped<IUnityOfWork, UnityOfWork>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services;
    }
}
