using Domain.Enums;

namespace Domain.Entities;

public class Sync
{
  public required ProgressStatus Status { get; set; }
  public required DateTimeOffset StartedAt { get; set; }
  public DateTimeOffset? EndedAt { get; set; }
}
