using Application.DTO.Market;
using Application.Interfaces;
using Binance.Spot;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Infrastructure;

internal class Binance(HttpClient client, IMemoryCache memoryCache) : IMarket
{
  private const string SYMBOLS_PRICES_CACHE_KEY = "SYMBOLS_PRICES_CACHE_KEY";

  public async Task<IEnumerable<SymbolPrice>> GetPrices(IEnumerable<string> symbols)
  {
    var all = await GetAllSymbolPricesFromCache();
    return all.Where(s => symbols.Contains(s.Symbol));
  }

  private async Task<IEnumerable<SymbolPrice>> GetAllSymbolPricesFromCache()
  {
    if (!memoryCache.TryGetValue(SYMBOLS_PRICES_CACHE_KEY, out IEnumerable<SymbolPrice>? allSymbols))
    {
      allSymbols = await GetAllSymbolPrices();

      memoryCache.Set(SYMBOLS_PRICES_CACHE_KEY, allSymbols,
        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
    }

    return allSymbols!;
  }

  private async Task<IEnumerable<SymbolPrice>> GetAllSymbolPrices()
  {
    var market = new Market(client);
    var allSymbolsString = await market.SymbolPriceTicker();
    return JsonConvert.DeserializeObject<IEnumerable<SymbolPrice>>(allSymbolsString);
  }
}
