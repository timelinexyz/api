using Core.Filters;
using Domain.Enums;

namespace Domain.Payloads;

public class WalletFilter
{
  public StringFilter? Name { get; set; }
  public EnumFilter<WalletType>? Type { get; set; }
  public bool IsValid() => Name is not null || Type is not null;
}
