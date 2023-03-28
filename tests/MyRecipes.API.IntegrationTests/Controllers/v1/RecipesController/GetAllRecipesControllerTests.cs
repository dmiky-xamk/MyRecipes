using FluentAssertions;
using MyRecipes.API.IntegrationTests.Helpers;
using MyRecipes.Application.Recipes.Queries;
using System.Net.Http.Json;

namespace MyRecipes.API.IntegrationTests.Controllers.v1.RecipesController;

[Collection("Test collection")]
public class GetAllRecipesControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabaseAsync;

    public GetAllRecipesControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Get_ReturnsEmpty_WhenUserHasNoRecipes()
    {
        // Arrange
        var (token, _) = await _factory.CreateUser("test@test.com", "Test123");
        _client.AddAuthorizationHeader(token);

        // Act
        var response = await _client.GetAsync(Routes.V1.Recipes);
        var recipes = await response.Content.ReadFromJsonAsync<IEnumerable<QueryRecipeDto>>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        recipes.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_ReturnsOnlyCurrentUserRecipes_WhenMultipleUsersHaveRecipes()
    {
        // Arrange
        var firstUser = await _factory.CreateUser("test@test.com", "Test123");
        var firstUserRecipeId = "123";
        
        _client.AddAuthorizationHeader(firstUser.token);
        await _factory.CreateRecipe(firstUserRecipeId, firstUser.userId);

        var secondUser = await _factory.CreateUser("test2@test.com", "Test246");
        var secondUserRecipeId = "246";
        
        _client.AddAuthorizationHeader(secondUser.token);
        var expectedRecipe = await _factory.CreateRecipe(secondUserRecipeId, secondUser.userId);

        // Act
        var response = await _client.GetAsync(Routes.V1.Recipes);
        var recipes = await response.Content.ReadFromJsonAsync<IEnumerable<QueryRecipeDto>>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        recipes.Should().ContainEquivalentOf(expectedRecipe, opt => opt.ExcludingMissingMembers());
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