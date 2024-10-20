using Core;
using Domain.Enums;
using FluentValidation;

namespace Application.Txns.Queries.Search;

public sealed class SearchTxnsQueryValidator : AbstractValidator<SearchTxnsQuery>
{
  public SearchTxnsQueryValidator()
  {
    RuleFor(q => q.Filter.PageNumber)
      .GreaterThanOrEqualTo(1)
      .WithMessage("{PropertyName} must be greater than or equal to 1.");

    RuleFor(q => q.Filter.PageSize)
      .GreaterThanOrEqualTo(1)
      .WithMessage("{PropertyName} must be greater than or equal to 1.")
      .LessThanOrEqualTo(100)
      .WithMessage("{PropertyName} must be less than or equal to 100.");

    //RuleFor(q => q.Filter.Date)
    //  .Must(d => d.IsValid())
    //  .WithMessage("{PropertyName} must have at least Before or After set.")
    //  .When(q => q.Filter.Date is not null);

    //RuleFor(q => q.Filter.Status)
    //  .Must(s => s != TxnStatus.None)
    //  .WithMessage("{PropertyName} must be explicitly set, it cannot be None.")
    //  .When(q => q.Filter.Status.HasValue);

    //When(q => q.Filter.Category is not null, () => {
    //  RuleFor(q => q.Filter.Category)
    //    .Must(c => !c.Types!.IsNullOrEmpty() || !c.Subtypes!.IsNullOrEmpty() || !c.Labels!.IsNullOrEmpty())
    //    .WithMessage("{PropertyName} must have at least one of Types/Subtypes/Labels fields set.");

    //  RuleFor(q => q.Filter.Category!.Types)
    //    .Must(t => t.All(type => !string.IsNullOrWhiteSpace(type)))
    //    .WithMessage("{PropertyName} cannot have null/empty/whitespace values.")
    //    .When(q => !q.Filter.Category!.Types!.IsNullOrEmpty());

    //  RuleFor(q => q.Filter.Category!.Subtypes)
    //    .Must(t => t.All(subtype => !string.IsNullOrWhiteSpace(subtype)))
    //    .WithMessage("{PropertyName} cannot have null/empty/whitespace values.")
    //    .When(q => !q.Filter.Category!.Subtypes!.IsNullOrEmpty());

    //  RuleFor(q => q.Filter.Category!.Labels)
    //    .Must(t => t.All(label => !string.IsNullOrWhiteSpace(label)))
    //    .WithMessage("{PropertyName} cannot have null/empty/whitespace values.")
    //    .When(q => !q.Filter.Category!.Labels!.IsNullOrEmpty());
    //});
  }
}
