using Application.Interfaces;
using Binance.Spot;

namespace Infrastructure;

internal class BinancePriceProvider(HttpClient client) : IPriceProvider
{
  public async Task<string> GetPrices(string symbols)
  {
    var market = new Market(client);

    return await market.SymbolPriceTicker(symbols);
  }
}
