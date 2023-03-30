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
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IDbConnectionFactory, PostgresConnectionFactory>();
        services.AddScoped<IApplicationDbContextInitializer, ApplicationDbContextInitializer>();
        services.AddScoped<IDataAccess, PostgreSqlDataAccess>();
        services.AddScoped<ICrud, PostgreSqlCrud>();
        services.AddDbContext<ApplicationDbContext>();

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}