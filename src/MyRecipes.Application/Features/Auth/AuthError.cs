namespace MyRecipes.Application.Features.Auth;

public enum AuthError
{
    InvalidCredentials,
    EmailAlreadyTaken,
    Unknown
}

public class AuthenticationError
{
    public AuthenticationError(AuthError errorType, string errorMessage)
    {
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    public AuthError ErrorType { get; set; }
    public string ErrorMessage { get; set; }
}