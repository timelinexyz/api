﻿using Domain.Common;
using Domain.Enums;

namespace Domain.Payloads;

public class TxnPatch : Entity<string>
{
  public string? ParentID { get; set; }
  public TxnStatus? Status { get; set; }
  public string? Type { get; set; }
  public string? Subtype { get; set; }
  public string? Description { get; set; }
  public IEnumerable<string>? Labels { get; set; }
}