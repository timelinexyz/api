using Core;
using Domain.Enums;
using System.Diagnostics;

namespace Domain.Entities;

[DebuggerDisplay("{Symbol}")]
public class Currency : Entity<int>
{
  public required CurrencyType Type { get; set; }
  public required string Name { get; set; }
  public required string Symbol { get; set; }
}
