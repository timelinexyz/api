using Core.Filters;
using Domain.Enums;

namespace Domain.Payloads;

public class TxnFilter : PaginationFilter
{
  public StringFilter? ID { get; set; }
  public StringFilter? ParentID { get; set; }
  public DateRangeFilter? Date { get; set; }
  public TxnStatus? Status { get; set; }
  public CategoryFilter? Category { get; set; }
  public AmountFilter? From { get; set; }
  public AmountFilter? To { get; set; }
  public AmountFilter? Fee { get; set; }
  public DecimalRangeFilter? NetValue { get; set; }
  public DecimalRangeFilter? FeeValue { get; set; }
  public StringFilter? Description { get; set; }
  public bool? Margin { get; set; }
}
