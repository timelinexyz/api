using Domain.Enums;

namespace Domain.Payloads;

// TODO: include relational operators (contains, starts with, is, is not, etc) that make sense for each data type
public class TxnFilter : PaginationFilter
{
  public string? ParentID { get; set; }
  public DateRange? Date { get; set; }
  public TxnStatus? Status { get; set; }
  public CategoryFilter? Category { get; set; }
  public CurrencyFilter? From {  get; set; }
  public CurrencyFilter? To { get; set; }
  public CurrencyFilter? Fee { get; set; }
  public NumberRange<decimal>? NetValue { get; set; }
  public NumberRange<decimal>? FeeValue { get; set; }
  public StringFilter? Description { get; set; }
  public bool? Margin { get; set; }
}
