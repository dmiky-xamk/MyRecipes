using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace MyRecipes.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddFluentValidation();

        services.ConfigureAPIVersioning();
        services.ConfigureSwaggerVersioning();
        services.ConfigureSwaggerPage();

        services.AddEndpointsApiExplorer();

        return services;
    }

    private static IServiceCollection ConfigureAPIVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new(1, 0);
        });

        return services;
    }

    private static IServiceCollection ConfigureSwaggerVersioning(this IServiceCollection services)
    {
        services.AddVersionedApiExplorer(opt =>
        {
            // Major, Minor, Patch
            opt.GroupNameFormat = "'v'VVV";

            // Swagger knows the version to use.
            opt.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static IServiceCollection ConfigureSwaggerPage(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            var title = "My Recipes API";
            var description = "A web API that allows users to submit and view their favorite recipes.";

            opt.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = $"{title} v1",
                Description = description
            });
        });

        return services;
    }

}