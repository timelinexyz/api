using Application.DTO.Market;

namespace Application.Interfaces;

public interface IMarket
{
  Task<IEnumerable<SymbolPrice>> GetPrices(IEnumerable<string> symbols);
}
