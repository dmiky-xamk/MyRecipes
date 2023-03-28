using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.IntegrationTests.Helpers;
using MyRecipes.Application.Features.Auth.Register;
using MyRecipes.Application.Users;
using System.Net;
using System.Net.Http.Json;

namespace MyRecipes.API.IntegrationTests.Controllers.v1.AccountController;

[Collection("Test collection")]
public class UserControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabaseAsync;

    public UserControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Get_ReturnsUserData_WhenValidToken()
    {
        // Arrange
        var email = "test@test.com";
        var password = "Test123";
        var recipeId = "123";

        var (token, userId) = await _factory.CreateUser(email, password);
        _client.AddAuthorizationHeader(token);

        await _factory.CreateRecipe(recipeId, userId);

        // Act
        var response = await _client.GetAsync(Routes.V1.Account);
        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.OK);
        content!.Token.Should().NotBeNullOrWhiteSpace();
        content.Email.Should().Be(email);
        content.Recipes.Should().ContainSingle();
    }

    [Fact]
    public async Task Post_ReturnsProblemDetails_WhenInvalidToken()
    {
        // Arrange
        var expectedErrorTitle = "Unauthorized";

        var invalidToken = "token";
        _client.AddAuthorizationHeader(invalidToken);

        // Act
        var response = await _client.GetAsync(Routes.V1.Account);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        content.Should().BeEquivalentTo(new
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = expectedErrorTitle,
        }, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Post_ReturnsProblemDetails_WhenEmailIsAlreadyTaken()
    {
        // Arrange
        var expectedErrorTitle = "The email has already been taken.";

        var email = "test@test.com";
        var password = "Test123";
        var credentials = new RegisterDto(email, password);
        await _factory.CreateUser(email, password);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.AccountRegister, credentials);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.Conflict);
        content.Should().BeEquivalentTo(new
        {
            Status = StatusCodes.Status409Conflict,
            Title = expectedErrorTitle,
        }, opt => opt.ExcludingMissingMembers());
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabaseAsync();
}