namespace Domain.Entities;

public class Category
{
  public required string Type { get; set; }
  public string? Subtype { get; set; }
  public IEnumerable<string>? Labels { get; set; }
}
