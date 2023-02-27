using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipes.Application.Infrastructure.Identity;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Infrastructure.Identity;
using MyRecipes.Infrastructure.Persistence;

namespace MyRecipes.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrasctuctureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IDataAccess, PostgreSqlDataAccess>();
        services.AddScoped<ICrud, PostgreSqlCrud>();

        services.ConfigureDbContext(config);
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            //opt.UseSqlite(config.GetConnectionString("Default"),
            //    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            
            opt.UseNpgsql(config.GetConnectionString("Postgre"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        return services;
    }
}