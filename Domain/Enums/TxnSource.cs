using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum TxnSource
{
  None,
  Manual,
  Import,
  Sync
}
