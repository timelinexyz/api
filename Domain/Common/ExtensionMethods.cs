namespace Domain.Common;

public static class ExtensionMethods
{
  public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
  {
    return source is null || !source.Any();
  }
}
