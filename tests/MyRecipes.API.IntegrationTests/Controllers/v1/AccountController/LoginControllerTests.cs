using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.API.IntegrationTests.Helpers;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.API.IntegrationTests.Controllers.v1.AccountController;
[Collection("Test collection")]
public class LoginControllerTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabaseAsync;

    public LoginControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Post_ReturnsUserData_WhenCorrectCredentials()
    {
        // Arrange
        var email = "test@test.com";
        var password = "Test123";

        var (_, userId) = await _factory.CreateUser(email, password);
        var credentials = new LoginDto(email, password);

        var recipeId = "123";
        await _factory.CreateRecipe(recipeId, userId);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.AccountLogin, credentials);
        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        content!.Token.Should().NotBeNullOrWhiteSpace();
        content.Email.Should().Be(email);
        content.Recipes.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task Post_ReturnsProblemDetails_WhenInvalidEmail()
    {
        // Arrange
        var email = "test@test.com";
        var password = "Test123";

        var (_, userId) = await _factory.CreateUser(email, password);

        var wrongEmail = "abc@test.com";
        var credentials = new LoginDto(wrongEmail, password);

        var recipeId = "123";
        await _factory.CreateRecipe(recipeId, userId);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.AccountLogin, credentials);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Unauthorized);
        content.Should().BeOfType<ProblemDetails>();
    }
    
    [Fact]
    public async Task Post_ReturnsProblemDetails_WhenInvalidPassword()
    {
        // Arrange
        var email = "test@test.com";
        var password = "Test123";

        var (_, userId) = await _factory.CreateUser(email, password);

        var wrongPassword = "Password123";
        var credentials = new LoginDto(email, wrongPassword);

        var recipeId = "123";
        await _factory.CreateRecipe(recipeId, userId);

        // Act
        var response = await _client.PostAsJsonAsync(Routes.V1.AccountLogin, credentials);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Unauthorized);
        content.Should().BeOfType<ProblemDetails>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabaseAsync();
}