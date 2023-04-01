using Microsoft.Extensions.DependencyInjection;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Infrastructure.Identity;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Infrastructure.Identity;
using MyRecipes.Infrastructure.Persistence;

namespace MyRecipes.Application.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IDbConnectionFactory, PostgresConnectionFactory>();
        services.AddScoped<IApplicationDbContextInitializer, ApplicationDbContextInitializer>();
        
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddDbContext<ApplicationDbContext>();

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}