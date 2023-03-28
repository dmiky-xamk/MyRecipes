using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.IntegrationTests.Helpers;
using MyRecipes.Application.Features.Recipes;
using MyRecipes.Application.Recipes.Queries;
using System.Net.Http.Json;

namespace MyRecipes.API.IntegrationTests.Controllers.v1.RecipesController;

[Collection("Test collection")]
public class UpdateRecipeControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabaseAsync;

    public UpdateRecipeControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _resetDatabaseAsync = _factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Update_ReturnsUpdatedRecipe_WhenUpdateSucceeds()
    {
        // Arrange
        var (token, userId) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var testRecipeId = "123";
        var createdRecipe = (await _factory.CreateRecipe(testRecipeId, userId)).ToQueryRecipeDto();

        var updatedRecipe = new RecipeDto("Updated test recipe name", "", "", createdRecipe.Ingredients, createdRecipe.Directions);

        // Act
        var response = await _client.PutAsJsonAsync(Routes.V1.Recipes + $"/{testRecipeId}", updatedRecipe);
        var actualRecipe = await response.Content.ReadFromJsonAsync<QueryRecipeDto>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        actualRecipe.Should().BeEquivalentTo(updatedRecipe);
    }

    [Fact]
    public async Task Update_ReturnsValidationErrors_WhenInvalidRecipe()
    {
        // Arrange
        var (token, userId) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var testRecipeId = "123";
        var createdRecipe = (await _factory.CreateRecipe(testRecipeId, userId)).ToQueryRecipeDto();

        var invalidNameRecipe = new RecipeDto("", "", "", createdRecipe.Ingredients, createdRecipe.Directions);

        // Act
        var response = await _client.PutAsJsonAsync(Routes.V1.Recipes + $"/{testRecipeId}", invalidNameRecipe);
        var actualRecipe = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.UnprocessableEntity);
        actualRecipe.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Update_ReturnsProblemDetails_WhenInvalidRecipeId()
    {
        // Arrange
        var (token, userId) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var testRecipeId = "123";
        var invalidRecipeId = "246";
        var createdRecipe = (await _factory.CreateRecipe(testRecipeId, userId)).ToQueryRecipeDto();

        var updatedRecipe = new RecipeDto("Updated test recipe name", "", "", createdRecipe.Ingredients, createdRecipe.Directions);

        // Act
        var response = await _client.PutAsJsonAsync(Routes.V1.Recipes + $"/{invalidRecipeId}", updatedRecipe);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.NotFound);
        content.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Update_ReturnsUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var (token, userId) = await _factory.CreateUser("test@test.com", "Test123");

        var testRecipeId = "123";
        var createdRecipe = (await _factory.CreateRecipe(testRecipeId, userId)).ToQueryRecipeDto();

        var updatedRecipe = new RecipeDto("Updated test recipe name", "", "", createdRecipe.Ingredients, createdRecipe.Directions);

        // Act
        var response = await _client.PutAsJsonAsync(Routes.V1.Recipes + $"/{testRecipeId}", updatedRecipe);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Unauthorized);
        content.Should().BeEmpty();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabaseAsync();
}