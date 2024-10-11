using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Txn : AuditEntity<string>
{
  public string? ParentID { get; set; }
  public required DateTimeOffset Date { get; set; }
  public required TxnStatus Status { get; set; }
  public required Category Category { get; set; }
  public required TxnAmount From { get; set; }
  public TxnAmount? To { get; set; }
  public TxnAmount? Fee { get; set; }
  public required decimal NetValue { get; set; }
  public required decimal FeeValue { get; set; }
  public required bool Margin { get; set; }
  public string? Description { get; set; }
}
