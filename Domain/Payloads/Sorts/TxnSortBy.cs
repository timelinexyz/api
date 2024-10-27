﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Payloads;

[JsonConverter(typeof(StringEnumConverter))]
public enum TxnSortBy
{
  None,
  Date,
  Pnl,
  NetValue,
}
