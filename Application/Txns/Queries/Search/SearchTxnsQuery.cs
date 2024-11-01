using Application.Interfaces;
using Application.Interfaces.Common;
using Core;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Payloads;

namespace Application.Txns.Queries.Search;

public sealed record SearchTxnsQuery(TxnFilter Filter, TxnSort? Sort) : IQuery<IPaginatedList<Txn>>;

internal sealed class SearchTxnsQueryHandler(ITxnRepository txnRepository, IPnl pnl) : IQueryHandler<SearchTxnsQuery, IPaginatedList<Txn>>
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

    await pnl.CalculatePnlAgainstLatestPrice(page.Records.Where(t => t.Status == TxnStatus.Open && t.Category.Type == TxnType.Buy.ToString()));

    return Result.Create(page);
  }
}