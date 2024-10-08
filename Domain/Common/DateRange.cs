namespace Domain.Common;

public class DateRange
{
  public DateTime? After { get; set; }
  public DateTime? Before { get; set; }

  public bool IsValid() => After.HasValue || Before.HasValue;
  public bool IsInvalid() => !IsValid();
  public bool IsFullyDefined() => After.HasValue && Before.HasValue;

  public override string ToString()
  {
    if (IsInvalid())
    {
      throw new InvalidOperationException("Invalid time period. At least one property must have value.");
    }

    if (After.HasValue && Before.HasValue)
    {
      return $"After: {After.Value:d MMMM yyyy HH:mm:ss}, Before: {Before.Value:d MMMM yyyy HH:mm:ss}";
    }
    else if (After.HasValue)
    {
      return $"After: {After.Value:d MMMM yyyy HH:mm:ss}";
    }
    else
    {
      return $"Before: {Before!.Value:d MMMM yyyy HH:mm:ss}";
    }
  }
}
