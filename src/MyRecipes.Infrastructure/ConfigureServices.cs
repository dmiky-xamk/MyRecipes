using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Infrastructure.Identity;
using MyRecipes.Infrastructure.Persistence;

namespace MyRecipes.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrasctuctureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IDataAccess, SqliteDataAccess>();
        services.AddScoped<ICrud, Crud>();

        services.ConfigureDbContext(config);
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("Default"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        return services;
    }
}