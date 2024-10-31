using Domain.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Koinly;

internal static class Extensions
{
  public static Txn[] GetKoinlyTxns()
  {
    return ReadAllKoinlyTransactionFiles().DoSomeAnalysis().Select(txn => txn.Convert()).ToArray();
  }

  private static KoinlyTxn[] DoSomeAnalysis(this KoinlyTxn[] txns)
  {
    var txnTypes = new HashSet<string>();
    var txnLabels = new HashSet<string>();
    var currencyTypes = new HashSet<string>();
    var currencySubtypes = new HashSet<string>();
    var walletTypes = new HashSet<string>();

    var uniqueTxns = new HashSet<KoinlyTxn>(txns, new KoinlyTxnEqualityComparer());

    foreach (var txn in uniqueTxns)
    {
      if (!string.IsNullOrWhiteSpace(txn.type))
      {
        txnTypes.Add(txn.type);
      }

      if (!string.IsNullOrWhiteSpace(txn.label))
      {
        txnLabels.Add(txn.label);
      }

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
}
