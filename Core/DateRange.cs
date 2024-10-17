namespace Core;

public class DateRange
{
  public DateTimeOffset? After { get; set; }
  public DateTimeOffset? Before { get; set; }

  public bool IsInvalid() => !IsPartiallyDefined();
  public bool IsPartiallyDefined() => After.HasValue || Before.HasValue;
  public bool IsFullyDefined() => After.HasValue && Before.HasValue;

  public override string ToString()
  {
    if (After.HasValue && Before.HasValue)
    {
      return $"After: {After.Value:d MMMM yyyy HH:mm:ss}, Before: {Before.Value:d MMMM yyyy HH:mm:ss}";
    }
    if (After.HasValue)
    {
      return $"After: {After.Value:d MMMM yyyy HH:mm:ss}";
    }
    if (Before.HasValue)
    {
      return $"Before: {Before!.Value:d MMMM yyyy HH:mm:ss}";
    }

    throw new InvalidOperationException("Both After and Before are null.");
  }
}
