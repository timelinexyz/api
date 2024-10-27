using Application.Interfaces.Common;
using Core;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Payloads;

namespace Application.Txns.Commands.GroupTxns;

public sealed record GroupTxnsCommand(string ParentID, IEnumerable<string> ChildIDs) : ICommand;

internal sealed class GroupTxnsCommandHandler(ITxnRepository txnRepository) : ICommandHandler<GroupTxnsCommand>
{
  public async Task<Result> Handle(GroupTxnsCommand request, CancellationToken cancellationToken)
  {
    //var parentTxn = await FindTxn(request.ParentID);

    //if (parentTxn is null)
    //{
    //  return DomainErrors.Txn.NotFound;
    //}

    //var tasks = request.ChildIDs.Select(id => txnRepository.Patch(new TxnPatch
    //{
    //  ID = id,
    //  ParentID = request.ParentID,
    //}));

    //await Task.WhenAll(tasks);

    return Result.Success();
  }

  //private async Task<Txn?> FindTxn(string id)
  //{
  //  var filter = new TxnFilter
  //  {
  //    PageNumber = 1,
  //    PageSize = 1,
  //    ID = id
  //  };

  //  var txns = await txnRepository.Search(filter, sort: null);

  //  if (txns.TotalRecords > 1)
  //  {
  //    throw new InvalidOperationException($"There are {txns.TotalRecords} transactions with the same ID: {id}.");
  //  }

  //  return txns.Records.FirstOrDefault();
  //}
}