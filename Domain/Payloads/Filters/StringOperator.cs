using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Payloads;

[JsonConverter(typeof(StringEnumConverter))]
public enum StringOperator
{
  None,
  Contains,
  StartsWith,
  EndsWith,
}
