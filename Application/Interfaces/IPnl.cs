using Domain.Entities;

namespace Application.Interfaces;

internal interface IPnl
{
  Task CalculatePnlAgainstLatestPrice(Txn txn);
  Task CalculatePnlAgainstLatestPrice(IEnumerable<Txn> txns);
  void CalculatePnlAgainstPrice(Txn txn, decimal price);
  void CalculatePnlAgainstPrice(IEnumerable<Txn> txns, decimal price);
}
