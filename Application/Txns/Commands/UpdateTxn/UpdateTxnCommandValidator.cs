using FluentValidation;

namespace Application.Txns.Commands.UpdateTxn;

//public sealed record UpdateTxnCommand(
//  string? ID,
//  string? ParentID,
//  TxnStatus? Status,
//  UpdateCategoryDto? Category,
//  string? Description) : ICommand;

//public sealed record UpdateCategoryDto(
//  string? Type,
//  string? Subtype,
//  IEnumerable<string>? Labels);

public sealed class UpdateTxnCommandValidator : AbstractValidator<UpdateTxnCommand>
{
  public UpdateTxnCommandValidator()
  {
    RuleFor(c => c.ID)
      .NotNull()
      .WithMessage("{PropertyName} is required.")
      .NotEmpty()
      .WithMessage("{PropertyName} must not be empty.")
      .Must(id => !id?.Any(x => char.IsWhiteSpace(x)) ?? false)
      .WithMessage("{PropertyName} must not contain whitespace characters.");

    RuleFor(c => c)
      .Must(c =>
        !string.IsNullOrWhiteSpace(c.ParentID) ||
        c.Status.HasValue ||
        c.Category is not null ||
        !string.IsNullOrWhiteSpace(c.Description))
      .WithMessage("No txn field to update.");
  }
}