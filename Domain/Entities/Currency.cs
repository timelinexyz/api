using Core;
using Domain.Enums;

namespace Domain.Entities;

public class Currency : Entity<int>
{
  public required CurrencyType Type { get; set; }
  public required string Name { get; set; }
  public required string Symbol { get; set; }
}
