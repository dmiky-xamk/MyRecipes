using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.IntegrationTests.Helpers;
using System.Net.Http.Json;

namespace MyRecipes.API.IntegrationTests.Controllers.v1.RecipesController;

[Collection("Test collection")]
public class DeleteRecipeControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabaseAsync;

    public DeleteRecipeControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleteSucceeds()
    {
        // Arrange
        var (token, userId) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var testRecipeId = "123";
        await _factory.CreateRecipe(testRecipeId, userId);

        // Act
        var response = await _client.DeleteAsync(Routes.V1.Recipes + $"/{testRecipeId}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.NoContent);
        content.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_ReturnsProblemDetails_WhenInvalidRecipeId()
    {
        // Arrange
        var (token, userId) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var testRecipeId = "123";
        var invalidRecipeId = "246";
        await _factory.CreateRecipe(testRecipeId, userId);

        // Act
        var response = await _client.DeleteAsync(Routes.V1.Recipes + $"/{invalidRecipeId}");
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.NotFound);
        content.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Delete_ReturnsUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var (_, userId) = await _factory.CreateUser("test@test.com", "Test123");

        var testRecipeId = "123";
        await _factory.CreateRecipe(testRecipeId, userId);

        // Act
        var response = await _client.DeleteAsync(Routes.V1.Recipes + $"/{testRecipeId}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Unauthorized);
        content.Should().BeEmpty();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabaseAsync();
}