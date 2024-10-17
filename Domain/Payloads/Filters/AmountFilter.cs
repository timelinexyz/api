namespace Domain.Payloads;

public class AmountFilter
{
  public CurrencyFilter? Currency { get; set; }
  public WalletFilter? Wallet { get; set; }
}
