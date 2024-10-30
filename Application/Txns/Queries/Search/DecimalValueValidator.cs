using Core.Filters;
using FluentValidation;

namespace Application.Txns.Queries.Search;

public sealed class DecimalValueValidator : AbstractValidator<DecimalRangeFilter>
{
  public DecimalValueValidator()
  {
    RuleFor(f => f)
      .Must(f => f.GreaterThan != default || f.LessThan != default)
      .WithMessage("{PropertyName} must have at least one of GreaterThan/LessThan fields set.");

    RuleFor(f => f)
      .Must(f => f.GreaterThan < f.LessThan)
      .WithMessage("When both GreaterThan and LessThan are set, GreaterThan must be less than LessThan.")
      .When(f => f.GreaterThan != default && f.LessThan != default);
  }
}
