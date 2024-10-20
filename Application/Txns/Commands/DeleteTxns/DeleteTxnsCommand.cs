using Application.Interfaces.Common;
using Core;
using Domain.Interfaces;

namespace Application.Txns.Commands.DeleteTxns;

public sealed record DeleteTxnsCommand(IEnumerable<TxnId> Records) : ICommand;
public sealed record TxnId(string ID);

internal sealed class DeleteTxnsCommandHandler(ITxnRepository txnRepository) : ICommandHandler<DeleteTxnsCommand>
{
  public async Task<Result> Handle(DeleteTxnsCommand request, CancellationToken cancellationToken)
  {
    await txnRepository.Delete(request.Records.Select(t => t.ID));

    return Result.Success();
  }
}