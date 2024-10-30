using Core.Filters;
using Domain.Enums;

namespace Domain.Payloads;

public class CurrencyFilter
{
  public EnumFilter<CurrencyType>? Type { get; set; }
  public StringFilter? Symbol { get; set; }
  public bool IsValid() => Type is not null || Symbol is not null;
}
