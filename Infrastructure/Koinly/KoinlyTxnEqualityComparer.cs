using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Koinly
{
  internal class KoinlyTxnEqualityComparer : IEqualityComparer<KoinlyTxn>
  {
    public bool Equals(KoinlyTxn? x, KoinlyTxn? y)
    {
      if (x is null || y is null)
        return false;

      return x.id == y.id;
    }

    public int GetHashCode([DisallowNull] KoinlyTxn obj)
    {
      return obj.id.GetHashCode();
    }
  }
}
