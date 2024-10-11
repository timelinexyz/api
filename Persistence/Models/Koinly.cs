namespace Persistence.Models;

internal class KoinlyTxnPage
{
  public KoinlyTxn[] transactions { get; set; }
}

internal class KoinlyTxn
{
  public string id { get; set; }
  public DateTimeOffset? date { get; set; }
  public string? type { get; set; }
  public string? label { get; set; }
  public KoinlyAmout From { get; set; }
  public KoinlyAmout To { get; set; }
  public KoinlyAmout Fee { get; set; }
  public decimal? net_value { get; set; }
  public decimal? fee_value { get; set; }
  public bool? margin { get; set; }
}

internal class KoinlyAmout
{
  public decimal? amount { get; set; }
  public KoinlyCurrency? currency { get; set; }
  public KoinlyWallet? wallet { get; set; }
  public decimal? cost_basis { get; set; }
}

internal class KoinlyCurrency
{
  public string? type { get; set; }
  public string? subtype { get; set; }
  public string? name { get; set; }
  public string? symbol { get; set; }
}

internal class KoinlyWallet
{
  public string? name { get; set; }
  public string? wallet_type { get; set; }
}
