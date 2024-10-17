namespace Core.Filters;

public class StringFilter
{
  public required Operator Operator { get; set; }
  public required HashSet<string> Values { get; set; }
  public bool Not { get; set; }

  public bool ApplyFilter(string value)
  {
    return Operator switch
    {
      Operator.Equals => !Not && value.Equals(Values.First(), StringComparison.OrdinalIgnoreCase),
      Operator.Contains => !Not && value.Contains(Values.First(), StringComparison.OrdinalIgnoreCase),
      Operator.StartsWith => !Not && value.StartsWith(Values.First(), StringComparison.OrdinalIgnoreCase),
      Operator.EndsWith => !Not && value.EndsWith(Values.First(), StringComparison.OrdinalIgnoreCase),
      Operator.In => !Not && Values.Contains(value),
      _ => throw new InvalidOperationException($"Operator: {Operator} is not supported against a single string."),
    };
  }

  public bool ApplyFilter(IEnumerable<string> values)
  {
    return Operator switch
    {
      Operator.Any => !Not && (Values.IsNullOrEmpty() || Values.Overlaps(values)), // TODO: ex. get all TXNs that have ANY label??
      Operator.All => !Not && Values.IsSubsetOf(values),
      _ => throw new InvalidOperationException($"Operator: {Operator} is not supported against a string array."),
    };
  }
}
