using Application.DTO.Market;

namespace Application.Interfaces;

public interface IMarket
{
  Task<IEnumerable<CandleStickData>> KlineCandlestickData(string symbol, DateTimeOffset startTime, DateTimeOffset endTime);
  Task<IEnumerable<SymbolPrice>> GetPrices(IEnumerable<string> symbols);
}
