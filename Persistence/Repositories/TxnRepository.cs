using Core;
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
    if (filter.Date is not null)
    {
      query = query.Where(t => filter.Date.ApplyFilter(t.Date));
    }

    // Status
    if (filter.Status.HasValue)
    {
      query = query.Where(t => t.Status == filter.Status.Value);
    }

    // Category
    if (filter.Category is not null)
    {
      if (filter.Category.Type is not null)
      {
        query = query.Where(t => filter.Category.Type.ApplyFilter(t.Category.Type));
      }
      if (filter.Category.Subtype is not null)
      {
        query = query.Where(t => filter.Category.Subtype.ApplyFilter(t.Category.Subtype));
      }
      if (filter.Category.Labels is not null)
      {
        query = query.Where(t => t.Category.Labels != null && filter.Category.Labels.ApplyFilter(t.Category.Labels));
      }
    }

    // From
    if (filter.From is not null)
    {
      if (filter.From.Currency is not null)
      {
        if (filter.From.Currency.Type is not null)
        {
          query = query.Where(t => filter.From.Currency.Type.ApplyFilter(t.From.Currency.Type));
        }
        if (filter.From.Currency.Symbol is not null)
        {
          query = query.Where(t => filter.From.Currency.Symbol.ApplyFilter(t.From.Currency.Symbol));
        }
      }
      if (filter.From.Wallet is not null)
      {
        if (filter.From.Wallet.Name is not null)
        {
          query = query.Where(t => filter.From.Wallet.Name.ApplyFilter(t.From.Wallet.Name));
        }
        if (filter.From.Wallet.Type is not null)
        {
          query = query.Where(t => filter.From.Wallet.Type.ApplyFilter(t.From.Wallet.Type));
        }
      }
    }

    // To
    if (filter.To is not null)
    {
      if (filter.To.Currency is not null)
      {
        if (filter.To.Currency.Type is not null)
        {
          query = query.Where(t => filter.To.Currency.Type.ApplyFilter(t.To.Currency.Type)); // TODO: null check????
        }
        if (filter.To.Currency.Symbol is not null)
        {
          query = query.Where(t => filter.To.Currency.Symbol.ApplyFilter(t.To.Currency.Symbol));
        }
      }
      if (filter.To.Wallet is not null)
      {
        if (filter.To.Wallet.Name is not null)
        {
          query = query.Where(t => filter.To.Wallet.Name.ApplyFilter(t.To.Wallet.Name));
        }
        if (filter.To.Wallet.Type is not null)
        {
          query = query.Where(t => filter.To.Wallet.Type.ApplyFilter(t.To.Wallet.Type));
        }
      }
    }

    // Fee
    if (filter.Fee is not null)
    {
      if (filter.Fee.Currency is not null)
      {
        if (filter.Fee.Currency.Type is not null)
        {
          query = query.Where(t => filter.Fee.Currency.Type.ApplyFilter(t.Fee.Currency.Type)); // TODO: null check????
        }
        if (filter.Fee.Currency.Symbol is not null)
        {
          query = query.Where(t => filter.Fee.Currency.Symbol.ApplyFilter(t.Fee.Currency.Symbol));
        }
      }
      if (filter.Fee.Wallet is not null)
      {
        if (filter.Fee.Wallet.Name is not null)
        {
          query = query.Where(t => filter.Fee.Wallet.Name.ApplyFilter(t.Fee.Wallet.Name));
        }
        if (filter.Fee.Wallet.Type is not null)
        {
          query = query.Where(t => filter.Fee.Wallet.Type.ApplyFilter(t.Fee.Wallet.Type));
        }
      }
    }

    // Net Value
    if (filter.NetValue is not null)
    {
      query = query.Where(t => filter.NetValue.ApplyFilter(t.NetValue));
    }

    // Fee Value
    if (filter.FeeValue is not null)
    {
      query = query.Where(t => filter.FeeValue.ApplyFilter(t.FeeValue));
    }

    // Description
    if (filter.Description is not null)
    {
      query = query.Where(t => filter.Description.ApplyFilter(t.Description));
    }

    // Margin
    if (filter.Margin.HasValue)
    {
      query = query.Where(t => t.Margin == filter.Margin.Value);
    }

    // Sorting
    // TODO: Sorting
    query = query.OrderByDescending(t => t.Date);

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
