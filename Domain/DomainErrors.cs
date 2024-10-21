using Core;

namespace Domain;

public static class DomainErrors
{
  public static class Txn
  {
    public static readonly Error NotFound = new(
        "Txn.NotFound",
        "No transactions found.");
  }
}
