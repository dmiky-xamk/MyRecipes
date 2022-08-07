using MyRecipes.Application.Common.Enums;

namespace MyRecipes.Application.Common.Models;

public abstract class BaseResult<T>
{
    public bool IsSuccess { get; set; }
    public T Value { get; set; } = default!;
    public string Error { get; set; } = string.Empty;
}

public class Result<T> : BaseResult<T>
{
    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
}

public class IdentificationResult<T> : BaseResult<T>
{
    public IdentificationError IdentityError { get; set; }
    public (string, string) ErrorState { get; set; }
    public string UserId { get; set; } = string.Empty;

    public static IdentificationResult<T> Success(T value, string userId = "")
    {
        return new()
        {
            IsSuccess = true,
            UserId = userId,
            Value = value 
        };
    }

    public static IdentificationResult<T> Failure(IdentificationError error)
    {
        return Failure(error, string.Empty);
    }

    public static IdentificationResult<T> Failure(IdentificationError error, string errorMessage) 
    {
        return new() 
        {
            IdentityError = error,
            Error = errorMessage
        };
    }

    public static IdentificationResult<T> Failure(IdentificationError error, (string key, string value) errorState)
    {
        return Failure(error, errorState.key, errorState.value);
    }

    public static IdentificationResult<T> Failure(IdentificationError error, string key, string value)
    {
        return new()
        {
            IsSuccess = false,
            IdentityError = error,
            ErrorState = (key, value)
        };
    }
}