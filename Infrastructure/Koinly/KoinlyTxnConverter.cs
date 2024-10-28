﻿using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Koinly;

internal static class KoinlyTxnConverter
{
  public static Txn Convert(this KoinlyTxn txn)
  {
    var category = new Category
    {
      Type = txn.GetTxnType(),
      Labels = string.IsNullOrWhiteSpace(txn.label) ? [] : [txn.label],
    };

    var status = TxnStatus.Undefined;

    if (category.Type == TxnType.Buy.ToString())
    {
      status = TxnStatus.Open;
    }
    if (category.Type == TxnType.Sell.ToString())
    {
      status = TxnStatus.Closed;
    }

    return new Txn
    {
      ID = txn.id,
      CreatedAt = DateTimeOffset.UtcNow,
      Date = txn.date!.Value,
      Status = status,
      Category = category,
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
        Symbol = amount.currency.symbol.ToUpper()
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

  private static string GetTxnType(this KoinlyTxn txn)
  {
    var fromType = txn.from?.currency?.type;
    var toType = txn.to?.currency?.type;

    if (!string.IsNullOrWhiteSpace(fromType) && !string.IsNullOrWhiteSpace(toType))
    {
      return GetTxnType(fromType, toType);
    }
    else
    {
      return txn.type;
    }
  }

  private static string GetTxnType(string fromCurrency, string toCurrency)
  {
    if (fromCurrency.Equals(CurrencyType.Fiat.ToString(), StringComparison.OrdinalIgnoreCase))
    {
      if (toCurrency.Equals(CurrencyType.Crypto.ToString(), StringComparison.OrdinalIgnoreCase))
      {
        return TxnType.Buy.ToString();
      }
      else
      {
        return TxnType.Exchange.ToString();
      }
    }

    if (fromCurrency.Equals(CurrencyType.Stablecoin.ToString(), StringComparison.OrdinalIgnoreCase))
    {
      if (toCurrency.Equals(CurrencyType.Crypto.ToString(), StringComparison.OrdinalIgnoreCase))
      {
        return TxnType.Buy.ToString();
      }
      else
      {
        return TxnType.Exchange.ToString();
      }
    }

    if (fromCurrency.Equals(CurrencyType.Crypto.ToString(), StringComparison.OrdinalIgnoreCase))
    {
      if (toCurrency.Equals(CurrencyType.Fiat.ToString(), StringComparison.OrdinalIgnoreCase))
      {
        return TxnType.Sell.ToString();
      }
      if (toCurrency.Equals(CurrencyType.Stablecoin.ToString(), StringComparison.OrdinalIgnoreCase))
      {
        return TxnType.Sell.ToString();
      }
      else
      {
        return TxnType.Exchange.ToString();
      }
    }

    throw new InvalidOperationException("Unsupported Txn.From/Txn.To currency type.");
  }
}