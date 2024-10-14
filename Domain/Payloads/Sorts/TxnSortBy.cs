using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Payloads;

[JsonConverter(typeof(StringEnumConverter))]
public enum TxnSortBy
{
  None,
  CreatedAt,
  ModifiedAt,
  Date,
  Pnl,
  Amount
}
