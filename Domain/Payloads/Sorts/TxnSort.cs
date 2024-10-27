namespace Domain.Payloads;

public class TxnSort
{
  public required TxnSortBy By { get; set; }
  public required SortOrder Order { get; set; }
}
