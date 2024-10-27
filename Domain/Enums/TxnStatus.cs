using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum TxnStatus
{
  /// <summary>
  /// Undefined.
  /// </summary>
  Undefined,

  /// <summary>
  /// Pnl is calculated against the latest currency price.
  /// </summary>
  Open,

  /// <summary>
  /// Pnl is calculated against the sell price.
  /// </summary>
  Closed
}
