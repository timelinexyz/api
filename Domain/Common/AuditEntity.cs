namespace Domain.Common;

public abstract class AuditEntity<T> : Entity<T>
{
  public required DateTimeOffset CreatedAt { get; set; }
  public required DateTimeOffset ModifiedAt { get; set; }
}
