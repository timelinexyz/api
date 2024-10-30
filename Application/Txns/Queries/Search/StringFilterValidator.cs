using Core;
using Core.Filters;
using FluentValidation;

namespace Application.Txns.Queries.Search;

public sealed class StringFilterValidator : AbstractValidator<StringFilter>
{
  private static readonly HashSet<Operator> _validSingleValueOperators = [Operator.IsNull, Operator.Equals, Operator.Contains, Operator.StartsWith, Operator.EndsWith, Operator.In];

  public StringFilterValidator()
  {
    RuleFor(f => f.Operator)
      .Must(o => _validSingleValueOperators.Contains(o))
      .WithMessage($"Invalid Operator. Allowed values: {string.Join(", ", _validSingleValueOperators.Select(o => o.ToString()))}");

    RuleFor(f => f.Values)
      .Must(v => !v.IsNullOrEmpty() && v.Count == 1 && !string.IsNullOrWhiteSpace(v.First()))
      .WithMessage("Values must contain a single string value that is not null/empty/whitespace.")
      .When(f => f.Operator is not Operator.In);

    RuleFor(f => f.Values)
      .Must(v => !v.IsNullOrEmpty() && v.All(l => !string.IsNullOrWhiteSpace(l)))
      .WithMessage("Values must contain at least a single string value and cannot have null/empty/whitespace values.")
      .When(f => f.Operator is Operator.In);
  }
}
