using Application.Exceptions;
using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

internal class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
  : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (!validators.IsNullOrEmpty())
    {
      var context = new ValidationContext<TRequest>(request);
      var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
      
      var errors = validationResults
        .Where(r => !r.IsValid)
        .SelectMany(r => r.Errors)
        .Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
        .ToList();

      if (!errors.IsNullOrEmpty())
      {
        throw new Exceptions.ValidationException(errors);
      }
    }

    return await next();
  }
}