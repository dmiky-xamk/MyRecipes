using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.IntegrationTests.Helpers;
using MyRecipes.Application.Features.Recipes;
using System.Net.Http.Json;
using MyRecipes.Application.Features.Recipes.Dtos;

namespace MyRecipes.API.IntegrationTests.Controllers.v1.RecipesController;

[Collection("Test collection")]
public class CreateRecipeControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabaseAsync;

    public CreateRecipeControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _resetDatabaseAsync = _factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Create_ShouldCreateRecipe()
    {
        // Arrange
        var (token, userId) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var ingredients = new List<IngredientDto>() { new IngredientDto("Test ingredient", "", "") };
        var directions = new List<DirectionDto>() { new DirectionDto("Test direction") };
        var expectedRecipe = new RecipeDto("Test recipe", "", "", ingredients, directions);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.Recipes, expectedRecipe);
        var actualRecipe = await response.Content.ReadFromJsonAsync<QueryRecipeDto>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Created);
        actualRecipe.Should().BeEquivalentTo(expectedRecipe, opt => opt.ExcludingMissingMembers());
    }
    
    [Fact]
    public async Task Create_ShouldReturnValidationErrors_WhenInvalidRecipe()
    {
        // Arrange
        var (token, _) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var ingredientsWithEmptyName = new List<IngredientDto>() { new IngredientDto("", "", "") };
        var directions = new List<DirectionDto>() { new DirectionDto("Test direction") };
        var recipeWithEmptyName = new RecipeDto("", "", "", ingredientsWithEmptyName, directions);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.Recipes, recipeWithEmptyName);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.UnprocessableEntity);
        content.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Create_ReturnsUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var ingredients = new List<IngredientDto>() { new IngredientDto("Test ingredient", "", "") };
        var directions = new List<DirectionDto>() { new DirectionDto("Test direction") };
        var recipe = new RecipeDto("Test recipe", "", "", ingredients, directions);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.Recipes, recipe);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Unauthorized);
        content.Should().BeEmpty();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabaseAsync();
}