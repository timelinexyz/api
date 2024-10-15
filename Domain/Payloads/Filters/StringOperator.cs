using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Payloads;

[JsonConverter(typeof(StringEnumConverter))]
public enum StringOperator
{
  None,
  Equals,
  Contains,
  StartsWith,
  EndsWith
}
