using Domain.Enums;
namespace Domain.Payloads;

public class CurrencyFilter
{
  public IEnumerable<CurrencyType>? Types { get; set; }
  public IEnumerable<string>? Symbols { get; set; }
}
