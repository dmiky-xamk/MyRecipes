namespace MyRecipes.Application.Common.Enums;

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