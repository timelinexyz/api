using Domain.Common;

namespace Domain.Entities;

public class Wallet : AuditEntity<string>
{
  public required string Name { get; set; }
  public required string Type { get; set; }
  public Sync? Sync { get; set; }
}
