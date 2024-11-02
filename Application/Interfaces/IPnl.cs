using Domain.Entities;

namespace Application.Interfaces;

internal interface IPnl
{
  Task CalculatePnlAgainstLatestPrice(Txn buyTxn);
  Task CalculatePnlAgainstLatestPrice(IEnumerable<Txn> buyTxns);
  void CalculatePnl(IEnumerable<Txn> buyTxns, Txn sellTxn);
}
