using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyRecipes.Application.Entities;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Infrastructure.Persistence;
using MyRecipes.Domain.Entities;
using MyRecipes.Infrastructure.Identity;

namespace MyRecipes.API.IntegrationTests.Helpers;

public static class Utilities
{
    public static async Task<(string token, string userId)> CreateUser(this CustomWebApplicationFactory factory, string email, string password)
    {
        var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ApplicationUser user = new()
        {
            UserName = email,
            Email = email
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            var token = tokenService.GenerateToken(email, user.Id);

            return (token, user.Id);
        }

        var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));

        throw new Exception($"Unable to create {email}.{Environment.NewLine}{errors}");
    }

    public static async Task<RecipeEntity> CreateRecipe(this CustomWebApplicationFactory factory, string recipeId, string userId)
    {
        var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();

        var recipeRepository = scope.ServiceProvider.GetRequiredService<IRecipeRepository>();

        var recipe = new RecipeEntity()
        {
            Id = recipeId,
            Name = "Test recipe",
            Description = "Test description",
            UserId = userId,
            Ingredients = new List<IngredientEntity>()
                {
                    new(recipeId, "Test ingredient name", "", ""),
                },
            Directions = new List<DirectionEntity>()
                {
                    new(recipeId, "Test ingredient description"),
                }
        };

        await recipeRepository.CreateRecipeAsync(recipe);

        return recipe;
    }
}