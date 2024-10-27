namespace Application.Txns;

internal static class Extensions
{
  public static double PercentageChange(decimal v1, decimal v2)
  {
    return (double) Math.Round(((v2 - v1) / Math.Abs(v1) * 100), 3);
  }
}
