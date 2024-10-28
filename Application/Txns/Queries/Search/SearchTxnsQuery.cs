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

    return Result.Create(page);

    ////var pairs = page.Records
    ////  .Where(t => t.Category.Type == TxnType.Buy.ToString())
    ////  .Select(t => t.GetPair());

    //var prices = await market.GetPrices(["BTCEUR"]);

    //foreach (var txn in page.Records.Where(t => t.Category.Type == TxnType.Buy.ToString() && t.To.Currency.Symbol == "BTC"))
    //{
    //  //var pair = txn.GetPair();
    //  var price = prices.First();
    //  txn.Pnl = new Pnl
    //  {
    //    Value = price.Price * txn.To.Amount - txn.From.Amount,
    //    Percentage = Extensions.PercentageChange(txn.From.Amount, price.Price * txn.To.Amount),
    //  };
    //}

    //return Result.Create(page);
  }
}