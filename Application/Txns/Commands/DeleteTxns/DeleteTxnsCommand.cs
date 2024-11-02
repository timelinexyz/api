using Application.Interfaces.Common;
using Core;
using Domain.Interfaces;

namespace Application.Txns.Commands.DeleteTxns;

public sealed record DeleteTxnsCommand(IEnumerable<string> IDs) : ICommand;

internal sealed class DeleteTxnsCommandHandler(ITxnRepository txnRepository) : ICommandHandler<DeleteTxnsCommand>
{
  public async Task<Result> Handle(DeleteTxnsCommand request, CancellationToken cancellationToken)
  {
    await txnRepository.Delete(request.IDs.ToHashSet());

    return Result.Success();
  }
}