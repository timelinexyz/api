namespace Domain.Payloads;

public class StringFilter
{
  public required StringOperator Operator { get; set; }
  public required string Value { get; set; }
  public bool Not { get; set; }

  public bool Evaluate(string? operand)
  {
    if (string.IsNullOrWhiteSpace(operand))
    {
      return false;
    }

    return Operator switch
    {
      StringOperator.Equals => !Not && operand.Equals(Value, StringComparison.OrdinalIgnoreCase),
      StringOperator.Contains => !Not && operand.Contains(Value, StringComparison.OrdinalIgnoreCase),
      StringOperator.StartsWith => !Not && operand.StartsWith(Value, StringComparison.OrdinalIgnoreCase),
      StringOperator.EndsWith => !Not && operand.EndsWith(Value, StringComparison.OrdinalIgnoreCase),
      _ => throw new InvalidOperationException($"Invalid StringOperator: {Operator}."),
    };
  }
}
