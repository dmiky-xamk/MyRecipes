namespace MyRecipes.Application.Infrastructure.Identity;

public enum IdentificationError
{
    None,
    Unauthorized,
    ValidationProblem,
    UsernameTaken,
    EmailTaken,
    WrongCredentials,
    UserNotFound,
    UnknownError
}