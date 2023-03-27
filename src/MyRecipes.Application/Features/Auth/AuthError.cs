namespace MyRecipes.Application.Features.Auth;

public enum AuthError
{
    InvalidCredentials,
    EmailAlreadyTaken,
    Unknown
}

/// <summary>
/// The error response from the authentication endpoint when the authentication fails.
/// </summary>
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