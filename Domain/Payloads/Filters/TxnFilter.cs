using Domain.Common;
using Domain.Enums;

namespace Domain.Payloads;

public class TxnFilter : Pagination
{
  public IEnumerable<CurrencyType>? CurrencyTypes { get; set; }
  public IEnumerable<string>? Symbols { get; set; }
  public IEnumerable<string>? Labels { get; set; }
}
