namespace Core.Filters;

public class StringFilter
{
  public required Operator Operator { get; set; }
  public required HashSet<string> Values { get; set; }
  public bool Not { get; set; }

  public bool ApplyFilter(string? value) => Operator switch
  {
    Operator.IsNull => Not
      ? (value is not null)
      : (value is null),

    Operator.Equals => Not
      ? value is null || !value.Equals(Values.First(), StringComparison.OrdinalIgnoreCase)
      : value is not null && value.Equals(Values.First(), StringComparison.OrdinalIgnoreCase),

    Operator.Contains => Not
      ? value is null || !value.Contains(Values.First(), StringComparison.OrdinalIgnoreCase)
      : value is not null && value.Contains(Values.First(), StringComparison.OrdinalIgnoreCase),

    Operator.StartsWith => Not
      ? value is null || !value.StartsWith(Values.First(), StringComparison.OrdinalIgnoreCase)
      : value is not null && value.StartsWith(Values.First(), StringComparison.OrdinalIgnoreCase),

    Operator.EndsWith => Not
      ? value is null || !value.EndsWith(Values.First(), StringComparison.OrdinalIgnoreCase)
      : value is not null && value.EndsWith(Values.First(), StringComparison.OrdinalIgnoreCase),

    Operator.In => Not
      ? value is null || !Values.Contains(value)
      : value is not null && Values.Contains(value),

    _ => throw new InvalidOperationException($"Operator: {Operator} is not supported against a single string."),
  };

  public bool ApplyFilter(IEnumerable<string> values)
  {
    return Operator switch
    {
      Operator.Any => Not
        ? Values.IsNullOrEmpty() || !Values.Overlaps(values)
        : !Values.IsNullOrEmpty() && Values.Overlaps(values), // TODO: ex. get all TXNs that have ANY label??

      Operator.All => Not
        ? Values.IsNullOrEmpty() || !Values.IsSubsetOf(values)
        : !Values.IsNullOrEmpty() && Values.IsSubsetOf(values),

      _ => throw new InvalidOperationException($"Operator: {Operator} is not supported against a string array."),
    };
  }
}
