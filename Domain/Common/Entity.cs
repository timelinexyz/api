namespace Domain.Common;

public abstract class Entity
{
  public required string ID { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset LastModified { get; set; }
}
