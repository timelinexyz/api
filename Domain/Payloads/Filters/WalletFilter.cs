using Core.Filters;
using Domain.Enums;

namespace Domain.Payloads;

public class WalletFilter
{
  public StringFilter? Name { get; set; }
  public EnumFilter<WalletType>? Type { get; set; }
}
