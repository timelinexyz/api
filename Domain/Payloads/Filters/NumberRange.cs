namespace Domain.Payloads;

public class NumberRange<TNumber> where TNumber : struct
{
  public TNumber? GreaterThan { get; set; }
  public TNumber? LessThan { get; set; }
}
