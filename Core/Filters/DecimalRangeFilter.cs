namespace Core.Filters;

public class DecimalRangeFilter
{
  public decimal? GreaterThan { get; set; }
  public decimal? LessThan { get; set; }

  public bool ApplyFilter(decimal value)
  {
    if (GreaterThan != default && LessThan != default)
    {
      return value.CompareTo(GreaterThan) > 0 && value.CompareTo(LessThan) < 0;
    }
    if (GreaterThan != default)
    {
      return value.CompareTo(GreaterThan) > 0;
    }
    if (LessThan != default)
    {
      return value.CompareTo(LessThan) < 0;
    }

    throw new InvalidOperationException("Both GreaterThan and LessThan are null.");
  }
}