namespace Core.Filters;

public class NumberRangeFilter<TNumber> where TNumber : IComparable
{
  public TNumber? GreaterThan { get; set; }
  public TNumber? LessThan { get; set; }

  public bool ApplyFilter(TNumber value)
  {
    if (GreaterThan is not null && LessThan is not null)
    {
      return value.CompareTo(GreaterThan) > 0 && value.CompareTo(LessThan) < 0;
    }
    if (GreaterThan is not null)
    {
      return value.CompareTo(GreaterThan) > 0;
    }
    if (LessThan is not null)
    {
      return value.CompareTo(LessThan) < 0;
    }

    throw new InvalidOperationException("Both GreaterThan and LessThan are null.");
  }
}