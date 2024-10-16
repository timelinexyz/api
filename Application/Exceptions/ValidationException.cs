namespace Application.Exceptions;

public class ValidationException(IReadOnlyCollection<ValidationError> errors)
  : Exception("One or more validation failures have occurred.")
{
  public IReadOnlyCollection<ValidationError> Errors { get; } = errors;
}

public record ValidationError(string PropertyName, string ErrorMessage);
