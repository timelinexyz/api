using FluentValidation;

namespace Application.Txns.Search;

public sealed class SearchTxnsQueryValidator : AbstractValidator<SearchTxnsQuery>
{
  public SearchTxnsQueryValidator()
  {
    RuleFor(q => q.Filter.PageNumber)
      .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be greater than or equal to 1.");

    RuleFor(q => q.Filter.PageSize)
      .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be greater than or equal to 1.")
      .LessThanOrEqualTo(100).WithMessage("{PropertyName} must be less than or equal to 100.");

    // test with only pagination
  }
}
