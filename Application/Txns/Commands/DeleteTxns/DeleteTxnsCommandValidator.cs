using FluentValidation;

namespace Application.Txns.Commands.DeleteTxns;

public sealed class DeleteTxnsCommandValidator : AbstractValidator<DeleteTxnsCommand>
{
  public DeleteTxnsCommandValidator()
  {
    RuleFor(c => c.IDs)
      .NotNull()
      .WithMessage("{PropertyName} is required.")
      .NotEmpty()
      .WithMessage("{PropertyName} must not be empty.")
      .Must(ids => ids.All(id => !string.IsNullOrWhiteSpace(id)))
      .WithMessage("{PropertyName} must not contain null/empty/whitespace values.");
  }
}
