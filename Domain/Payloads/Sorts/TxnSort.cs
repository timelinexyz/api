namespace Domain.Payloads;

public class TxnSort
{
  public TxnSortBy SortBy { get; set; }
  public SortOrder SortOrder { get; set; }
}
