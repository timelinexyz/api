using FluentValidation;

namespace Application.Txns.Commands.GroupTxns;

public sealed class GroupTxnsCommandValidator : AbstractValidator<GroupTxnsCommand>
{
  public GroupTxnsCommandValidator()
  {
    RuleFor(c => c.ParentID)
      .NotNull()
      .WithMessage("{PropertyName} is required.")
      .NotEmpty()
      .WithMessage("{PropertyName} must not be empty.")
      .Must(id => !id?.Any(x => char.IsWhiteSpace(x)) ?? false)
      .WithMessage("{PropertyName} must not contain whitespace characters.");

    RuleFor(c => c.ChildIDs)
      .NotNull()
      .WithMessage("{PropertyName} is required.")
      .NotEmpty()
      .WithMessage("{PropertyName} must not be empty.")
      .Must(ids => ids.All(id => !string.IsNullOrWhiteSpace(id)))
      .WithMessage("{PropertyName} must not contain null/empty/whitespace values.");
  }
}
