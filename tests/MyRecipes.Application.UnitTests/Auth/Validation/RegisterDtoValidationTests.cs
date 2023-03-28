using FluentValidation.TestHelper;
using MyRecipes.Application.Features.Auth.Register;

namespace MyRecipes.Application.UnitTests.Auth.Validation;

public class RegisterDtoValidationTests
{
    private readonly RegisterDtoValidator _validator;

    public RegisterDtoValidationTests()
    {
        _validator = new RegisterDtoValidator();
    }

    [Fact]
    public void RegisterDto_ShouldHaveValidationError_WhenPasswordIsTooShort()
    {
        // Arrange
        var email = "test@test.com";
        var password = "Test";

        // Act
        var registerDto = new RegisterDto(email, password);
        var validationResult = _validator.TestValidate(registerDto);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
    }
    
    [Fact]
    public void RegisterDto_ShouldHaveValidationError_WhenPasswordDoesNotHaveNumber()
    {
        // Arrange
        var email = "test@test.com";
        var password = "Testing";

        // Act
        var registerDto = new RegisterDto(email, password);
        var validationResult = _validator.TestValidate(registerDto);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
    }
    
    [Fact]
    public void RegisterDto_ShouldHaveValidationError_WhenPasswordDoesNotHaveCapitalLetter()
    {
        // Arrange
        var email = "test@test.com";
        var password = "testing123";

        // Act
        var registerDto = new RegisterDto(email, password);
        var validationResult = _validator.TestValidate(registerDto);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
    }
    
    [Fact]
    public void RegisterDto_ShouldHaveValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var email = "test.com";
        var password = "Testing123";

        // Act
        var registerDto = new RegisterDto(email, password);
        var validationResult = _validator.TestValidate(registerDto);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(registerDto => registerDto.Email);
    }
}