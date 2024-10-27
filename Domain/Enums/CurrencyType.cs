using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum CurrencyType
{
  None,
  Crypto = 1,
  Stablecoin = 2,
  Fiat = 3
}
