namespace Domain.Payloads;

public class AmountFilter
{
  public CurrencyFilter? Currency { get; set; }
  public WalletFilter? Wallet { get; set; }
  public bool IsValid() => Currency is not null || Wallet is not null;
}
