using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyRecipes.API.Services;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Infrastructure.Identity;
using MyRecipes.Infrastructure.Persistence;
using System.Text;

namespace MyRecipes.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddControllers()
            .AddFluentValidation();

        services.ConfigureAPIVersioning();
        services.ConfigureSwaggerVersioning();
        services.ConfigureSwaggerPage();

        services.ConfigureAuthentication(config);

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

    private static IServiceCollection ConfigureAuthentication(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
        })
         .AddEntityFrameworkStores<ApplicationDbContext>()
         //.AddUserManager<UserManager<ApplicationUser>>()
         .AddSignInManager<SignInManager<ApplicationUser>>();

        // Allow the user to send the token back to us.
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    // What fields to validate in the token.
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,

                    // Validate the token against these values.
                    ValidIssuer = config.GetValue<string>("Authentication:Issuer"),
                    ValidAudience = config.GetValue<string>("Authentication:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        config.GetValue<string>("Authentication:SecretKey")))
                };
            });

        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}