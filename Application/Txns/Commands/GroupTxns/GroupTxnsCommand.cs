using Application.Interfaces;
using Application.Interfaces.Common;
using Core;
using Core.Filters;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Payloads;

namespace Application.Txns.Commands.GroupTxns;

public sealed record GroupTxnsCommand(string ParentID, IEnumerable<string> ChildIDs) : ICommand;

internal sealed class GroupTxnsCommandHandler(ITxnRepository txnRepository, IPnl pnl) : ICommandHandler<GroupTxnsCommand>
{
  public async Task<Result> Handle(GroupTxnsCommand request, CancellationToken cancellationToken)
  {
    var txnsResult = await GetTxns(request);

    if (txnsResult.IsFailure)
    {
      return txnsResult.Error;
    }

    // Parent TXN
    var parentTxn = txnsResult.Value.Single(t => t.ID == request.ParentID);
    
    //if (parentTxn.Status == TxnStatus.Closed)
    //{
    //  return Result.Failure(new Error("Txn.AlreadyClosed", "The parent TXN is already closed, cannot update it."));
    //}
    if (parentTxn.Category.Type != TxnType.Sell.ToString())
    {
      return Result.Failure(new Error("Txn.InvalidType", "The parent TXN type is not Sell, cannot Close it."));
    }
    if (parentTxn.To is null)
    {
      return Result.Failure(new Error("Txn.InvalidType", "The parent TXN does not have 'To', cannot Close it."));
    }

    // Children TXNs
    var childTxns = txnsResult.Value.Where(t => t.ID != request.ParentID).ToList();

    if (childTxns.Any(t => t.Status != TxnStatus.Open))
    {
      return Result.Failure(new Error("Txn.InvalidStatus", "All child TXN statuses must be Open."));
    }
    if (childTxns.Any(t => t.Category.Type != TxnType.Buy.ToString()))
    {
      return Result.Failure(new Error("Txn.InvalidType", "All child TXN types must be Buy."));
    }
    if (childTxns.Any(t => t.To is null))
    {
      return Result.Failure(new Error("Txn.InvalidType", "All child TXNs must have the 'To' field set."));
    }
    if (childTxns.Any(t => t.To!.Currency.Symbol != parentTxn.From.Currency.Symbol))
    {
      return Result.Failure(new Error("Txn.InvalidType", "All child TXNs must have the same 'To' currency as the parent TXN 'From'."));
    }

    // Child TXNs Pnl calculated against Parent TXN sell price.
    pnl.CalculatePnl(childTxns, parentTxn);

    var tasks = new List<Task>
    {
      txnRepository.Patch(
        new TxnPatch
        {
          ID = parentTxn.ID,
          Status = TxnStatus.Closed,
          Pnl = parentTxn.Pnl,
        })
    };

    tasks.AddRange(childTxns.Select(txn => txnRepository.Patch(
      new TxnPatch
      {
        ID = txn.ID,
        ParentID = request.ParentID,
        Status = TxnStatus.Closed,
        Pnl = txn.Pnl,
      })));

    await Task.WhenAll(tasks);

    return Result.Success();
  }

  private async Task<Result<IEnumerable<Txn>>> GetTxns(GroupTxnsCommand request)
  {
    var allIDs = request.ChildIDs.Append(request.ParentID).ToHashSet();

    var filter = new TxnFilter
    {
      PageNumber = 1,
      PageSize = allIDs.Count,
      ID = new StringFilter
      {
        Operator = Operator.In,
        Values = allIDs
      }
    };

    var sort = new TxnSort
    {
      By = TxnSortBy.Date,
      Order = SortOrder.Descending
    };

    var page = await txnRepository.Search(filter, sort);

    if (page.TotalRecords != allIDs.Count)
    {
      return Result.Failure<IEnumerable<Txn>>(DomainErrors.Txn.NotAllFound);
    }

    return Result.Success<IEnumerable<Txn>>(page.Records.ToList());
  }
}