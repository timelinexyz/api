using Application.Interfaces;
using Core;
using Domain.Entities;

namespace Application.Txns.Services;

internal sealed class PnlCalculator(IMarket market) : IPnl
{
  public async Task CalculatePnlAgainstLatestPrice(Txn txn)
  {
    if (txn.To is null)
    {
      throw new InvalidOperationException("Cannot calculate PnL without To.");
    }

    // TODO: This solution is hacky but IDK how to build the correct pair (BTCUSDC vs. USDCBTC)
    string[] pair = [txn.From.Currency.Symbol + txn.To.Currency.Symbol, txn.To.Currency.Symbol + txn.From.Currency.Symbol];

    if (pair.IsNullOrEmpty())
    {
      throw new InvalidOperationException("Txn has no pair.");
    }

    var prices = await market.GetPrices(pair);

    if (prices.IsNullOrEmpty())
    {
      throw new InvalidOperationException($"No price found for pair: {pair[0]} or {pair[1]}");
    }

    var price = prices.First();

    CalculatePnlAgainstPrice(txn, price.Price);
  }

  public async Task CalculatePnlAgainstLatestPrice(IEnumerable<Txn> txns)
  {
    foreach (var txn in txns)
    {
      await CalculatePnlAgainstLatestPrice(txn);
    }
  }

  public void CalculatePnlAgainstPrice(Txn txn, decimal price)
  {
    if (txn.To is null)
    {
      throw new InvalidOperationException("Cannot calculate PnL without To.");
    }

    var current = price * txn.To.Amount;

    txn.Pnl = new Pnl
    {
      Value = current,
      Delta = current - txn.From.Amount,
      Percentage = ExtensionMethods.PercentageChange(txn.From.Amount, current),
    };
  }

  public void CalculatePnlAgainstPrice(IEnumerable<Txn> txns, decimal price)
  {
    foreach (var txn in txns)
    {
      CalculatePnlAgainstPrice(txn, price);
    }
  }
}
