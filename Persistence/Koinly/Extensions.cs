using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;

namespace Persistence.Koinly;

internal static class Extensions
{
  public static KoinlyTransactions GetKoinlyTxns()
  {
    var txns = ReadAllKoinlyTransactionFiles().DoSomeAnalysis().Select(txn => txn.Convert()).ToArray();

    return new KoinlyTransactions
    {
      Txns = txns
    };
  }

  private static KoinlyTxn[] DoSomeAnalysis(this KoinlyTxn[] txns)
  {
    var currencyTypes = new HashSet<string>();
    var currencySubtypes = new HashSet<string>();
    var walletTypes = new HashSet<string>();

    var uniqueTxns = new HashSet<KoinlyTxn>(txns, new KoinlyTxnEqualityComparer());

    foreach (var txn in uniqueTxns)
    {
      // from
      if (txn.from is not null)
      {
        if (!string.IsNullOrWhiteSpace(txn.from.currency.type))
        {
          currencyTypes.Add(txn.from.currency.type);
        }
        if (!string.IsNullOrWhiteSpace(txn.from.currency.subtype))
        {
          currencySubtypes.Add(txn.from.currency.subtype);
        }

        walletTypes.Add(txn.from.wallet.wallet_type);
      }

      // to
      if (txn.to is not null)
      {
        if (!string.IsNullOrWhiteSpace(txn.to.currency.type))
        {
          currencyTypes.Add(txn.to.currency.type);
        }
        if (!string.IsNullOrWhiteSpace(txn.to.currency.subtype))
        {
          currencySubtypes.Add(txn.to.currency.subtype);
        }

        walletTypes.Add(txn.to.wallet.wallet_type);
      }

      // fee
      if (txn.fee is not null)
      {
        if (!string.IsNullOrWhiteSpace(txn.fee.currency.type))
        {
          currencyTypes.Add(txn.fee.currency.type);
        }
        if (!string.IsNullOrWhiteSpace(txn.fee.currency.subtype))
        {
          currencySubtypes.Add(txn.fee.currency.subtype);
        }

        walletTypes.Add(txn.fee.wallet.wallet_type);
      }
    }

    return uniqueTxns.ToArray();
  }

  private static KoinlyTxn[] ReadAllKoinlyTransactionFiles()
  {
    var path = "P:\\Portfolio Tracker\\koinly - all TXs";
    var pages = new List<KoinlyTxnPage>();

    foreach (string file in Directory.GetFiles(path, "*.json"))
    {
      string contents = File.ReadAllText(file);
      var page = JsonConvert.DeserializeObject<KoinlyTxnPage>(contents);
      pages.Add(page!);
    }

    return pages
      .SelectMany(p => p.transactions)
      .OrderByDescending(t => t.date)
      .ToArray();
  }

  private static Txn Convert(this KoinlyTxn txn)
  {
    return new Txn
    {
      ID = txn.id,
      CreatedAt = DateTimeOffset.UtcNow,
      Date = txn.date!.Value,
      Status = TxnStatus.None,
      Category = new Category
      {
        Type = txn.type,
        Subtype = txn.label,
      },
      From = txn.from.Convert(),
      To = txn.to.Convert(),
      Fee = txn.fee.Convert(),
      NetValue = txn.net_value ?? default,
      FeeValue = txn.fee_value ?? default,
      Margin = txn.margin ?? default
    };
  }

  private static TxnAmount Convert(this KoinlyAmount amount)
  {
    if (amount is null)
    {
      return null;
    }

    return new TxnAmount
    {
      Amount = amount.amount ?? default,
      Currency = new Currency
      {
        ID = 0,
        Type = amount.currency.GetCurrencyType(),
        Name = amount.currency.name,
        Symbol = amount.currency.symbol
      },
      Wallet = new WalletHeader
      {
        ID = string.Empty,
        Name = amount.wallet.name,
        Type = amount.wallet.GetWalletType(),
      },
      BaseCurrencyFactor = amount.cost_basis ?? default
    };
  }

  private static CurrencyType GetCurrencyType(this KoinlyCurrency currency)
  {
    if (currency is null)
    {
      return CurrencyType.None;
    }
    if (!string.IsNullOrWhiteSpace(currency.subtype))
    {
      return Enum.Parse<CurrencyType>(currency.subtype, ignoreCase: true);
    }
    else
    {
      return Enum.Parse<CurrencyType>(currency.type!, ignoreCase: true);
    }
  }

  private static WalletType GetWalletType(this KoinlyWallet wallet)
  {
    if (wallet is null || string.IsNullOrWhiteSpace(wallet.wallet_type))
    {
      return WalletType.None;
    }
    if (wallet.wallet_type.Equals("blockchain", StringComparison.OrdinalIgnoreCase))
    {
      return WalletType.SelfCustody;
    }
    if (wallet.wallet_type.Equals("exchange", StringComparison.OrdinalIgnoreCase))
    {
      return WalletType.Exchange;
    }

    return WalletType.Unknown;
  }
}
