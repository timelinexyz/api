namespace Domain.Common;

public abstract class Entity<T>
{
  public required T ID { get; set; }
}
