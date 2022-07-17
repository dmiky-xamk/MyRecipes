using Microsoft.Extensions.DependencyInjection;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Infrastructure.Persistence;

namespace MyRecipes.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrasctuctureServices(this IServiceCollection services)
    {
        services.AddScoped<IDataAccess, SqliteDataAccess>();
        services.AddScoped<ICrud, Crud>();

        return services;
    }
}