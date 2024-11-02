using Core;
using Domain.Enums;
using System.Diagnostics;

namespace Domain.Entities;

[DebuggerDisplay("{Name} - {Type}")]
public class WalletHeader : Entity<string>
{
  public required string Name { get; set; }
  public required WalletType Type { get; set; }
}
