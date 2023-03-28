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
            string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env is null)
            {
                throw new ApplicationException("'ASPNETCORE_ENVIRONMENT' is null.");
            }

            string connectionString = string.Empty;
            if (env == "Development")
            {
                connectionString = config.GetConnectionString("Postgre");
            }

            else
            {
                string? pgHost = Environment.GetEnvironmentVariable("PGHOST");
                string? pgPort = Environment.GetEnvironmentVariable("PGPORT");
                string? pgUser = Environment.GetEnvironmentVariable("PGUSER");
                string? pgPass = Environment.GetEnvironmentVariable("PGPASSWORD");
                string? pgDb = Environment.GetEnvironmentVariable("PGDATABASE");

                if (Enumerable.Any(new string?[] { pgHost, pgPort, pgUser, pgPass, pgDb }, s => s is null))
                {
                    throw new ApplicationException("Some of the database connection environment variables were null.");
                }

                connectionString = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";
            }

            opt.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        return services;
    }
}