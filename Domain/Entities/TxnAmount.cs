namespace Domain.Entities;

public class TxnAmount
{
  public required decimal Amount { get; set; }
  public required Currency Currency { get; set; }
  public required WalletHeader Wallet { get; set; }
  public required decimal BaseCurrencyFactor { get; set; }
}
