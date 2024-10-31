namespace Domain.Entities;

public class Pnl
{
  public required decimal Value { get; set; }
  public required decimal Delta { get; set; }
  public required double Percentage { get; set; }
}
