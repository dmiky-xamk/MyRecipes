using MyRecipes.Application.Infrastructure.Identity;

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

public class Result<T, U> : BaseResult<T>
{
    public U? ErrorValue { get; set; }
    public static Result<T, U> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T, U> Failure(string error, U errorValue) => new() { IsSuccess = false, Error = error, ErrorValue = errorValue };
}