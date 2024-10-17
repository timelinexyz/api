using Core;
using Domain.Enums;

namespace Domain.Entities;

public class WalletHeader : Entity<string>
{
  public required string Name { get; set; }
  public required WalletType Type { get; set; }
}
