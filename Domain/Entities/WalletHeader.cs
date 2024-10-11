using Domain.Common;

namespace Domain.Entities;

public class WalletHeader : Entity<string>
{
  public required string Name { get; set; }
  public required string Type { get; set; }
}
