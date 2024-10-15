using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Payloads;
using Persistence.Koinly;
using Persistence.Models;

namespace Persistence.Repositories;

internal sealed class TxnRepository(KoinlyTransactions txns) : ITxnRepository
{
  public async Task<IPaginatedList<Txn>> Search(TxnFilter filter, TxnSort sort)
  {
    var query = txns.Txns.AsEnumerable();

    // Parent
    if (!string.IsNullOrWhiteSpace(filter.ParentID))
    {
      query = query.Where(t => t.ParentID == filter.ParentID);
    }

    // Date
    if (filter.Date?.After.HasValue ?? false)
    {
      query = query.Where(t => t.Date > filter.Date.After.Value);
    }
    if (filter.Date?.Before.HasValue ?? false)
    {
      query = query.Where(t => t.Date <= filter.Date.Before.Value);
    }

    // Status
    if (filter.Status.HasValue)
    {
      query = query.Where(t => t.Status == filter.Status.Value);
    }

    // Category
    if (!filter.Category?.Types!.IsNullOrEmpty() ?? false)
    {
      query = query.Where(t => filter.Category!.Types!.Contains(t.Category.Type));
    }
    if (!filter.Category?.Subtypes!.IsNullOrEmpty() ?? false)
    {
      query = query.Where(t => !string.IsNullOrWhiteSpace(t.Category.Subtype) && filter.Category!.Subtypes!.Contains(t.Category.Subtype));
    }
    if (!filter.Category?.Labels!.IsNullOrEmpty() ?? false)
    {
      // TODO: many to many - All/Any ??
      query = query.Where(t => t.Category.Labels != null && filter.Category!.Labels!.ToHashSet().Overlaps(t.Category.Labels));
    }

    // From
    if (!filter.From?.Types!.IsNullOrEmpty() ?? false)
    {
      query = query.Where(t => filter.From!.Types!.Contains(t.From.Currency.Type));
    }
    if (!filter.From?.Symbols!.IsNullOrEmpty() ?? false)
    {
      query = query.Where(t => filter.From!.Symbols!.Contains(t.From.Currency.Symbol));
    }

    // To
    if (!filter.To?.Types!.IsNullOrEmpty() ?? false)
    {
      query = query.Where(t => t.To != null && filter.To!.Types!.Contains(t.To.Currency.Type));
    }
    if (!filter.To?.Symbols!.IsNullOrEmpty() ?? false)
    {
      query = query.Where(t => t.To != null && filter.To!.Symbols!.Contains(t.To.Currency.Symbol));
    }

    // Fee
    if (!filter.Fee?.Types!.IsNullOrEmpty() ?? false)
    {
      query = query.Where(t => t.Fee != null && filter.Fee!.Types!.Contains(t.Fee.Currency.Type));
    }
    if (!filter.Fee?.Symbols!.IsNullOrEmpty() ?? false)
    {
      query = query.Where(t => t.Fee != null && filter.Fee!.Symbols!.Contains(t.Fee.Currency.Symbol));
    }

    // Net Value
    if (filter.NetValue?.GreaterThan.HasValue ?? false)
    {
      query = query.Where(t => t.NetValue > filter.NetValue.GreaterThan.Value);
    }
    if (filter.NetValue?.LessThan.HasValue ?? false)
    {
      query = query.Where(t => t.NetValue < filter.NetValue.LessThan.Value);
    }

    // Fee Value
    if (filter.FeeValue?.GreaterThan.HasValue ?? false)
    {
      query = query.Where(t => t.FeeValue > filter.FeeValue.GreaterThan.Value);
    }
    if (filter.FeeValue?.LessThan.HasValue ?? false)
    {
      query = query.Where(t => t.FeeValue < filter.FeeValue.LessThan.Value);
    }

    // Description
    if (filter.Description is not null)
    {
      query = query.Where(t => filter.Description.Evaluate(t.Description));
    }

    // Margin
    if (filter.Margin.HasValue)
    {
      query = query.Where(t => t.Margin == filter.Margin.Value);
    }

    return PaginatedList<Txn>.Create(query, filter.PageNumber, filter.PageSize);
  }

  public async Task Patch(TxnPatch txn)
  {
    var existing = txns.Txns.Single(t => txn.ID == t.ID);

    if (!string.IsNullOrWhiteSpace(txn.ParentID))
    {
      existing.ParentID = txn.ParentID;
    }
    if (txn.Status.HasValue)
    {
      existing.Status = txn.Status.Value;
    }
    if (!string.IsNullOrWhiteSpace(txn.Type))
    {
      existing.Category.Type = txn.Type;
    }
    if (!string.IsNullOrWhiteSpace(txn.Subtype))
    {
      existing.Category.Subtype = txn.Subtype;
    }
    if (!string.IsNullOrWhiteSpace(txn.Description))
    {
      existing.Description = txn.Description;
    }
    if (!txn.Labels!.IsNullOrEmpty())
    {
      existing.Category.Labels = txn.Labels;
    }
  }

  public async Task Delete(IEnumerable<string> ids)
  {
    txns = new KoinlyTransactions
    {
      Txns = txns.Txns.Where(t => !ids.Contains(t.ID)).ToArray()
    };

    await Task.CompletedTask;
  }
}
