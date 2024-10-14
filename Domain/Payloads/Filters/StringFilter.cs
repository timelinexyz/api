namespace Domain.Payloads;

public class StringFilter
{
  public required StringOperator Operator { get; set; }
  public required string Value { get; set; }
}
