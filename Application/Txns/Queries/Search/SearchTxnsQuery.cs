using Application.Interfaces;
using Application.Interfaces.Common;
using Core;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Payloads;

namespace Application.Txns.Queries.Search;

public sealed record SearchTxnsQuery(TxnFilter Filter, TxnSort? Sort) : IQuery<IPaginatedList<Txn>>;

internal sealed class SearchTxnsQueryHandler(ITxnRepository txnRepository, IMarket market) : IQueryHandler<SearchTxnsQuery, IPaginatedList<Txn>>
{
  public async Task<Result<IPaginatedList<Txn>>> Handle(SearchTxnsQuery request, CancellationToken cancellationToken)
  {
    var sort = request.Sort;

    sort ??= new TxnSort
    {
      By = TxnSortBy.Date,
      Order = SortOrder.Descending
    };

    var page = await txnRepository.Search(request.Filter, sort);

    await CalculatePnlForOpenBuyTxns(page.Records);

    return Result.Create(page);
  }

  private async Task CalculatePnlForOpenBuyTxns(IEnumerable<Txn> txns)
  {
    if (txns.IsNullOrEmpty()) return;

    foreach (var txn in txns.Where(t => t.Status == TxnStatus.Open && t.Category.Type == TxnType.Buy.ToString()))
    {
      var pair = txn.GetPair();
      var prices = await market.GetPrices(pair);

      if (prices.IsNullOrEmpty())
      {
        throw new MissingFieldException($"No price found for pair: {pair[0]}");
      }

      var price = prices.First();

      txn.Pnl = new Pnl
      {
        Value = price.Price * txn.To!.Amount,
        Delta = price.Price * txn.To!.Amount - txn.From.Amount,
        Percentage = Extensions.PercentageChange(txn.From.Amount, price.Price * txn.To.Amount),
      };
    }
  }
}