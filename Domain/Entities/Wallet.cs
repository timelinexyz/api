using Core;
using Domain.Enums;

namespace Domain.Entities;

public class Wallet : AuditEntity<string>
{
  public required string Name { get; set; }
  public required WalletType Type { get; set; }
  public Sync? Sync { get; set; }
}
