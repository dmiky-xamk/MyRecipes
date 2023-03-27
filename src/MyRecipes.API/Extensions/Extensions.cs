using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using System.Text.Json;

namespace MyRecipes.API.Mapping;

public static class Extensions
{
    /// <summary>
    /// Adds validation errors from the <paramref name="validationResult"/> to the <paramref name="modelState"/>.
    /// </summary>
    /// <param name="validationResult"></param>
    /// <param name="modelState"></param>
    /// <returns><see cref="ModelStateDictionary"/> with the validation errors.</returns>
    public static ModelStateDictionary AddToModelState(this ValidationResult validationResult, ModelStateDictionary modelState)
    {
        foreach (var error in validationResult.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return modelState;
    }

    /// <summary>
    /// Configures exception middleware to handle exceptions and to return a <see cref="ProblemDetails"/> instance to the client.<br/>
    /// <see href="https://andrewlock.net/creating-a-custom-error-handler-middleware-function/"/>
    /// </summary>
    /// <param name="app">An instance of the <see cref="WebApplication"/> to apply the exception middleware to.</param>
    /// <param name="includeDetails">Whether to include the exception details in the <see cref="ProblemDetails"/> response.</param>
    public static void ConfigureExceptionHandler(this WebApplication app, bool includeDetails)
    {
        app.UseExceptionHandler(appError =>
        {
            // Provide a handling function directly.
            appError.Run(async context =>
            {
                var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionDetails is not null)
                {
                    // ProblemDetails has it's own content type.
                    context.Response.ContentType = "application/problem+json";

                    // Log

                    var title = includeDetails ? exceptionDetails.Error.Message : "An error occured";
                    var details = includeDetails ? exceptionDetails.Error.ToString() : null;

                    var problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = title,
                        Detail = details
                    };

                    var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                    if (traceId is not null)
                    {
                        problem.Extensions["traceId"] = traceId;
                    }

                    var stream = context.Response.Body;
                    await JsonSerializer.SerializeAsync(stream, problem);
                }
            });
        });
    }
}