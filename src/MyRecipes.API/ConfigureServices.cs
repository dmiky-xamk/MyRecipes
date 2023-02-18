using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyRecipes.API;
using MyRecipes.API.Features.Auth;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Infrastructure.Identity;
using MyRecipes.Infrastructure.Persistence;
using System.Reflection;
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
        services.ConfigureAuthorization();

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
        // Configure Swagger UI to represent the API more clearly.
        // https://code-maze.com/swagger-ui-asp-net-core-web-api/
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

            // Enable XML comments to document the API endpoints.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opt.IncludeXmlComments(xmlPath);

            // Add authentication to Swagger UI.
            // https://stackoverflow.com/questions/56234504/bearer-authentication-in-swagger-ui-when-migrating-to-swashbuckle-aspnetcore-ve
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization using the Bearer scheme.",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            });

            // Add authentication only to the endpoints that require it.
            // Excludes 'AllowAnonymous' endpoints --> login & register
            // https://stackoverflow.com/questions/59158352/swagger-ui-authentication-only-for-some-endpoints
            opt.OperationFilter<SecurityRequirementsOperationFilter>();
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
         .AddSignInManager<SignInManager<ApplicationUser>>();

        // Allow the user to send the token back to us.
        // The token is sent back in the 'Authorization' header.
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
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    private static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        // Require an authenticated user to access the endpoints.
        services.AddAuthorization(opt =>
        {
            opt.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}