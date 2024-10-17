namespace Core.Filters;

public class DateRangeFilter : DateRange
{
  public bool ApplyFilter(DateTimeOffset value)
  {
    if (After.HasValue && Before.HasValue)
    {
      return value > After.Value && value <= Before.Value;
    }
    if (After.HasValue)
    {
      return value > After.Value;
    }
    if (Before.HasValue)
    {
      return value <= Before.Value;
    }

    throw new InvalidOperationException("Both After and Before are null.");
  }
}