using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyRecipes.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        // Validator
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}