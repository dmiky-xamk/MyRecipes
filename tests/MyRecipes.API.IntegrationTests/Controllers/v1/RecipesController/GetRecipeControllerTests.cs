using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.IntegrationTests.Helpers;
using System.Net.Http.Json;
using MyRecipes.Application.Features.Recipes.Dtos;

namespace MyRecipes.API.IntegrationTests.Controllers.v1.RecipesController;

[Collection("Test collection")]
public class GetRecipeControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabaseAsync;

    public GetRecipeControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Get_ReturnsProblemDetails_WhenWrongRecipeId()
    {
        // Arrange
        var (token, _) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var testRecipeId = "123";

        // Act
        var response = await _client.GetAsync(Routes.V1.Recipes + $"/{testRecipeId}");
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.NotFound);
        content.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Get_ReturnsRecipe_WhenCorrectRecipeId()
    {
        // Arrange
        var (token, userId) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        var testRecipeId = "123";
        var expectedRecipe = await _factory.CreateRecipe(testRecipeId, userId);

        // Act
        var response = await _client.GetAsync(Routes.V1.Recipes + $"/{testRecipeId}");
        var actualRecipe = await response.Content.ReadFromJsonAsync<QueryRecipeDto>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        actualRecipe.Should().BeEquivalentTo(expectedRecipe, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Get_ReturnsUnauthorized_WhenNotAuthenticated()
    {
        // Act
        var response = await _client.GetAsync(Routes.V1.Recipes);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Unauthorized);
        content.Should().BeEmpty();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabaseAsync();
}