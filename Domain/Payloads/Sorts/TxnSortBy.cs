using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Payloads;

[JsonConverter(typeof(StringEnumConverter))]
public enum TxnSortBy
{
  None,
  CreatedAt,
  Pnl,
  Amount
}
