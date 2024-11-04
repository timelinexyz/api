using Application.DTO.Market;
using Application.Interfaces;
using Binance.Spot;
using Binance.Spot.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Infrastructure.Binance;

internal class Binance(HttpClient client, IMemoryCache memoryCache) : IMarket
{
  private readonly Market market = new(client);
  private const string SYMBOLS_PRICES_CACHE_KEY = "SYMBOLS_PRICES_CACHE_KEY";
  private const int PRICES_STALE_TIME_MINUTES = 5;

  public async Task<IEnumerable<CandleStickData>> KlineCandlestickData(string symbol, DateTimeOffset startTime, DateTimeOffset endTime)
  {
    var json = await market.KlineCandlestickData(symbol, Interval.ONE_DAY, startTime.ToUnixTimeMilliseconds(), endTime: endTime.ToUnixTimeMilliseconds());
    return JsonConvert.DeserializeObject<IEnumerable<object[]>>(json).Select(r => new CandleStickData(r));
  }

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
        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(PRICES_STALE_TIME_MINUTES)));
    }

    return allSymbols!;
  }

  private async Task<IEnumerable<SymbolPrice>> GetAllSymbolPrices()
  {
    var allSymbolsString = await market.SymbolPriceTicker();
    return JsonConvert.DeserializeObject<IEnumerable<SymbolPrice>>(allSymbolsString);
  }
}
