namespace Core.Filters;

public class EnumFilter<TEnum> where TEnum : Enum
{
  public required Operator Operator { get; set; }
  public required HashSet<TEnum> Values { get; set; }
  public bool Not { get; set; }

  public bool ApplyFilter(TEnum value)
  {
    return Operator switch
    {
      Operator.Equals => !Not && value.Equals(Values.First()),
      Operator.In => !Not && Values.Contains(value),
      _ => throw new InvalidOperationException($"Operator: {Operator} is not supported against a single enumeration."),
    };
  }
}
