using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.IntegrationTests.Helpers;
using MyRecipes.Application.Features.Auth.Register;
using MyRecipes.Application.Users;
using System.Net.Http.Json;

namespace MyRecipes.API.IntegrationTests.Controllers.v1.AccountController;

[Collection("Test collection")]
public class RegisterControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabaseAsync;

    public RegisterControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Post_ReturnsUserData_WhenCredentialValidationSucceeds()
    {
        // Arrange
        var email = "test@test.com";
        var password = "Test123";
        var credentials = new RegisterDto(email, password);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.AccountRegister, credentials);
        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        content!.Token.Should().NotBeNullOrWhiteSpace();
        content.Email.Should().Be(email);
        content.Recipes.Should().BeEmpty();
    }

    [Fact]
    public async Task Post_ReturnsProblemDetails_WhenCredentialValidationFails()
    {
        // Arrange
        var badEmail = "test.com";
        var badPassword = "test";
        var credentials = new RegisterDto(badEmail, badPassword);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.AccountRegister, credentials);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.UnprocessableEntity);
        content.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Post_ReturnsProblemDetails_WhenEmailIsAlreadyTaken()
    {
        // Arrange
        var email = "test@test.com";
        var password = "Test123";
        var credentials = new RegisterDto(email, password);
        await _factory.CreateUser(email, password);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.AccountRegister, credentials);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Conflict);
        content.Should().BeOfType<ProblemDetails>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabaseAsync();
}