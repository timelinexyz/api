namespace Domain.Common;

public abstract class AuditEntity<T> : Entity<T>
{
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset ModifiedAt { get; set; }
}
