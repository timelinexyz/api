using Application.Exceptions;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
  public async Task Invoke(HttpContext context)
  {
		try
		{
      await next(context);
    }
		catch (Exception ex)
		{
			logger.LogError(ex, "An unexpected exception has occured: {Message}", ex.Message);

      var exceptionDetails = GetExceptionDetails(ex);

      var problemDetails = new ProblemDetails
      {
        Status = exceptionDetails.Status,
        Title = exceptionDetails.Title,
        Detail = exceptionDetails.Detail,
      };

      if (!exceptionDetails.Errors!.IsNullOrEmpty())
      {
        problemDetails.Extensions["errors"] = exceptionDetails.Errors;
      }

      context.Response.StatusCode = exceptionDetails.Status;
      await context.Response.WriteAsJsonAsync(problemDetails);
    }
  }

  private static ExceptionDetails GetExceptionDetails(Exception exception)
  {
    return exception switch
    {
      ValidationException validationException => new ExceptionDetails(
        StatusCodes.Status400BadRequest,
        "Validation error",
        "One or more validation failures have occurred.",
        validationException.Errors),

      _ => new ExceptionDetails(
        StatusCodes.Status500InternalServerError,
        "Internal server error",
        "An unexpected error has occured.",
        null),
    };
  }

  internal record ExceptionDetails(
    int Status,
    string Title,
    string Detail,
    IEnumerable<object>? Errors);
}