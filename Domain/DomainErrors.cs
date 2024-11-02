using Core;

namespace Domain;

public static class DomainErrors
{
  public static class Txn
  {
    public static readonly Error NotFound = new(
        "Txn.NotFound",
        "No transactions found.");

    public static readonly Error NotAllFound = new(
        "Txn.NotAllFound",
        "Not all the specified transactions were found.");
  }
}
