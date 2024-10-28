using Core;
using Core.Filters;
using FluentValidation;

namespace Application.Txns.Queries.Search;

public sealed class SearchTxnsQueryValidator : AbstractValidator<SearchTxnsQuery>
{
  private static readonly HashSet<Operator> _allSingleValueOperators = [Operator.IsNull, Operator.Equals, Operator.Contains, Operator.StartsWith, Operator.EndsWith, Operator.In];
  private static readonly HashSet<Operator> _validSingleValueOperators = [Operator.IsNull, Operator.Equals, Operator.Contains, Operator.StartsWith, Operator.EndsWith];
  private static readonly HashSet<Operator> _validArrayConditionOperators = [Operator.IsNullOrEmpty, Operator.All, Operator.Any];

  public SearchTxnsQueryValidator()
  {
    // Pagination
    RuleFor(q => q.Filter.PageNumber)
      .GreaterThanOrEqualTo(1)
      .WithMessage("{PropertyName} must be greater than or equal to 1.");

    RuleFor(q => q.Filter.PageSize)
      .GreaterThanOrEqualTo(1)
      .WithMessage("{PropertyName} must be greater than or equal to 1.")
      .LessThanOrEqualTo(100)
      .WithMessage("{PropertyName} must be less than or equal to 100.");

    // ID
    When(q => q.Filter.ID is not null, () =>
    {
      When(q => q.Filter.ID!.Operator is not Operator.In, () =>
      {
        RuleFor(q => q.Filter.ID!.Operator)
          .Must(o => _validSingleValueOperators.Contains(o))
          .WithMessage($"Invalid Operator. Allowed values: {string.Join(", ", _allSingleValueOperators.Select(o => o.ToString()))}");

        RuleFor(q => q.Filter.ID!.Values)
          .Must(v => !v.IsNullOrEmpty() && v.Count == 1 && !string.IsNullOrWhiteSpace(v.First()))
          .WithMessage("Values must contain a single string value that is not null/empty/whitespace.");
      });

      RuleFor(q => q.Filter.ID!.Values)
        .Must(v => !v.IsNullOrEmpty() && v.All(l => !string.IsNullOrWhiteSpace(l)))
        .WithMessage("Values must contain at least a single string value and cannot have null/empty/whitespace values.")
        .When(q => q.Filter.ID!.Operator is Operator.In);
    });

    // Parent ID
    When(q => q.Filter.ParentID is not null, () =>
    {
      When(q => q.Filter.ParentID!.Operator is not Operator.In, () =>
      {
        RuleFor(q => q.Filter.ParentID!.Operator)
          .Must(o => _validSingleValueOperators.Contains(o))
          .WithMessage($"Invalid Operator. Allowed values: {string.Join(", ", _allSingleValueOperators.Select(o => o.ToString()))}");

        RuleFor(q => q.Filter.ParentID!.Values)
          .Must(v => !v.IsNullOrEmpty() && v.Count == 1 && !string.IsNullOrWhiteSpace(v.First()))
          .WithMessage("Values must contain a single string value that is not null/empty/whitespace.");
      });

      RuleFor(q => q.Filter.ParentID!.Values)
        .Must(v => !v.IsNullOrEmpty() && v.All(l => !string.IsNullOrWhiteSpace(l)))
        .WithMessage("Values must contain at least a single string value and cannot have null/empty/whitespace values.")
        .When(q => q.Filter.ParentID!.Operator is Operator.In);
    });

    // Date
    RuleFor(q => q.Filter.Date)
      .Must(d => !d!.IsInvalid())
      .WithMessage("{PropertyName} must have at least Before or After set.")
      .When(q => q.Filter.Date is not null);

    RuleFor(q => q.Filter.Date)
      .Must(d => d.After < d.Before)
      .WithMessage("When both Before and After are set, After must be less than Before.")
      .When(q => q.Filter.Date is not null && q.Filter.Date.IsFullyDefined());

    // Category
    When(q => q.Filter.Category is not null, () =>
    {
      RuleFor(q => q.Filter.Category)
        .Must(c => c.Type is not null || c.Labels is not null)
        .WithMessage("{PropertyName} must have at least one of Type/Labels fields set.");

      // Type
      When(q => q.Filter.Category!.Type is not null, () =>
      {
        When(q => q.Filter.Category!.Type!.Operator is not Operator.In, () =>
        {
          RuleFor(q => q.Filter.Category!.Type!.Operator)
            .Must(o => _validSingleValueOperators.Contains(o))
            .WithMessage($"Invalid Operator. Allowed values: {string.Join(", ", _allSingleValueOperators.Select(o => o.ToString()))}");

          RuleFor(q => q.Filter.Category!.Type!.Values)
            .Must(v => !v.IsNullOrEmpty() && v.Count == 1 && !string.IsNullOrWhiteSpace(v.First()))
            .WithMessage("Values must contain a single string value that is not null/empty/whitespace.");
        });

        RuleFor(q => q.Filter.Category!.Type!.Values)
          .Must(v => !v.IsNullOrEmpty() && v.All(l => !string.IsNullOrWhiteSpace(l)))
          .WithMessage("Values must contain at least a single string value and cannot have null/empty/whitespace values.")
          .When(q => q.Filter.Category!.Type!.Operator is Operator.In);
      });

      // Labels
      When(q => q.Filter.Category!.Labels is not null, () =>
      {
        RuleFor(q => q.Filter.Category!.Labels!.Operator)
          .Must(o => _validArrayConditionOperators.Contains(o))
          .WithMessage($"Invalid Operator. Allowed values: {string.Join(", ", _validArrayConditionOperators.Select(o => o.ToString()))}");

        RuleFor(q => q.Filter.Category!.Labels!.Values)
          .Must(v => !v.IsNullOrEmpty() && v.All(l => !string.IsNullOrWhiteSpace(l)))
          .WithMessage("Values must contain at least a single string value and cannot have null/empty/whitespace values.")
          .When(q => q.Filter.Category!.Labels!.Operator is Operator.Any or Operator.All);

        RuleFor(q => q.Filter.Category!.Labels!.Values)
          .Must(v => v.IsNullOrEmpty())
          .WithMessage("Values cannot contain any value.")
          .When(q => q.Filter.Category!.Labels!.Operator is Operator.IsNullOrEmpty);
      });
    });

    // From
    // To
    // Fee

    // Net Value
    RuleFor(q => q.Filter.NetValue)
      .Must(n => n.GreaterThan != default || n.LessThan != default)
      .WithMessage("{PropertyName} must have at least one of GreaterThan/LessThan fields set.")
      .When(q => q.Filter.NetValue is not null);

    RuleFor(q => q.Filter.NetValue)
      .Must(nv => nv.GreaterThan < nv.LessThan)
      .WithMessage("When both GreaterThan and LessThan are set, GreaterThan must be less than LessThan.")
      .When(q => q.Filter.NetValue is not null && (q.Filter.NetValue.GreaterThan != default && q.Filter.NetValue.LessThan != default));

    // Fee Value
    RuleFor(q => q.Filter.FeeValue)
      .Must(n => n.GreaterThan != default || n.LessThan != default)
      .WithMessage("{PropertyName} must have at least one of GreaterThan/LessThan fields set.")
      .When(q => q.Filter.FeeValue is not null);

    RuleFor(q => q.Filter.FeeValue)
      .Must(nv => nv.GreaterThan < nv.LessThan)
      .WithMessage("When both GreaterThan and LessThan are set, GreaterThan must be less than LessThan.")
      .When(q => q.Filter.FeeValue is not null && (q.Filter.FeeValue.GreaterThan != default && q.Filter.FeeValue.LessThan != default));

    // Description
    When(q => q.Filter.Description is not null, () =>
    {
      RuleFor(q => q.Filter.Description!.Operator)
          .Must(o => _validSingleValueOperators.Contains(o))
          .WithMessage($"Invalid Operator. Allowed values: {string.Join(", ", _validSingleValueOperators.Select(o => o.ToString()))}");

      RuleFor(q => q.Filter.Description!.Values)
        .Must(v => !v.IsNullOrEmpty() && v.Count == 1 && !string.IsNullOrWhiteSpace(v.First()))
        .WithMessage("Values must contain a single string value that is not null/empty/whitespace.");
    });
  }
}