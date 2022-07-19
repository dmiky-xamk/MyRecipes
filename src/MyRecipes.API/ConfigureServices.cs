using FluentValidation.AspNetCore;

namespace MyRecipes.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddFluentValidation();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}