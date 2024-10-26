using Application.Interfaces.Common;
using Core;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Payloads;

namespace Application.Txns.Commands.UpdateTxn;

public sealed record UpdateTxnCommand(
  string? ID,
  string? ParentID,
  TxnStatus? Status,
  UpdateCategoryDto? Category,
  string? Description) : ICommand;

public sealed record UpdateCategoryDto(
  string? Type,
  IEnumerable<string>? Labels);

internal sealed class UpdateTxnCommandHandler(ITxnRepository txnRepository) : ICommandHandler<UpdateTxnCommand>
{
  public async Task<Result> Handle(UpdateTxnCommand request, CancellationToken cancellationToken)
  {
    var txnPatch = new TxnPatch
    {
      ID = request.ID,
      ParentID = request.ParentID,
      Status = request.Status,
      Type = request.Category?.Type,
      Labels = request.Category?.Labels,
      Description = request.Description,
    };

    await txnRepository.Patch(txnPatch);

    return Result.Success();
  }
}
