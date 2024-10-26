namespace Domain.Entities;

public class Category
{
  public required string Type { get; set; }
  public IEnumerable<string>? Labels { get; set; }
}
