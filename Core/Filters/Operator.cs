﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Filters;

[JsonConverter(typeof(StringEnumConverter))]
public enum Operator
{
  None,

  // Single value
  Equals,
  Contains,
  StartsWith,
  EndsWith,

  // Array
  In,
  Any,
  All,
}