namespace Core;

public static class ExtensionMethods
{
  public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
  {
    return source is null || !source.Any();
  }

  public static double PercentageChange(decimal v1, decimal v2)
  {
    return (double)Math.Round(((v2 - v1) / Math.Abs(v1) * 100), 3);
  }
}
