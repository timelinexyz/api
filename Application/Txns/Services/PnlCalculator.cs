using Application.Interfaces;
using Core;
using Domain.Entities;

namespace Application.Txns.Services;

internal sealed class PnlCalculator(IMarket market) : IPnl
{
  public async Task CalculatePnlAgainstLatestPrice(Txn buyTxn)
  {
    if (buyTxn.To is null)
    {
      throw new InvalidOperationException("Cannot calculate PnL without To.");
    }

    // TODO: This solution is hacky but IDK how to build the correct pair (BTCUSDC vs. USDCBTC)
    string[] pair = [buyTxn.From.Currency.Symbol + buyTxn.To.Currency.Symbol, buyTxn.To.Currency.Symbol + buyTxn.From.Currency.Symbol];

    if (pair.IsNullOrEmpty())
    {
      throw new InvalidOperationException("Txn has no pair.");
    }

    var prices = await market.GetPrices(pair);

    if (prices.IsNullOrEmpty())
    {
      throw new InvalidOperationException($"No price found for pair: {pair[0]} or {pair[1]}");
    }

    CalculatePnlAgainstPrice(buyTxn, prices.First().Price);
  }

  public async Task CalculatePnlAgainstLatestPrice(IEnumerable<Txn> buyTxns)
  {
    foreach (var txn in buyTxns)
    {
      await CalculatePnlAgainstLatestPrice(txn);
    }
  }

  public void CalculatePnl(IEnumerable<Txn> buyTxns, Txn sellTxn)
  {
    CalculatePnlAgainstPrice(buyTxns, sellTxn.To!.Amount / sellTxn.From.Amount);

    var allTxnsDelta = buyTxns.Sum(t => t.Pnl!.Delta);

    sellTxn.Pnl = new Pnl
    {
      Value = sellTxn.To.Amount,
      Delta = allTxnsDelta,
      Percentage = ExtensionMethods.PercentageChange(sellTxn.To.Amount, sellTxn.To.Amount + allTxnsDelta),
    };
  }

  private static void CalculatePnlAgainstPrice(IEnumerable<Txn> txns, decimal price)
  {
    foreach (var txn in txns)
    {
      CalculatePnlAgainstPrice(txn, price);
    }
  }

  private static void CalculatePnlAgainstPrice(Txn txn, decimal price)
  {
    var current = price * txn.To!.Amount;

    txn.Pnl = new Pnl
    {
      Value = current,
      Delta = current - txn.From.Amount,
      Percentage = ExtensionMethods.PercentageChange(txn.From.Amount, current),
    };
  }
}
