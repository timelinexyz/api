using Core;
using Domain.Enums;
using System.Diagnostics;

namespace Domain.Entities;

[DebuggerDisplay("({Category.Type}) - {From.Amount} {From.Currency.Symbol} -> {To.Amount} {To.Currency.Symbol}")]
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
  public Pnl? Pnl { get; set; }
  public required bool Margin { get; set; }
  public string? Description { get; set; }

  // TODO: This solution is hacky but IDK how to build the correct pair (BTCUSDC vs. USDCBTC)
  public string[] GetPair()
  {
    if (To is null) return [];

    return [From.Currency.Symbol + To.Currency.Symbol, To.Currency.Symbol + From.Currency.Symbol];
  }
}
