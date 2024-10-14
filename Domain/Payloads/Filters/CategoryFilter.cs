namespace Domain.Payloads;

public class CategoryFilter
{
  public IEnumerable<string>? Types { get; set; }
  public IEnumerable<string>? Subtypes { get; set; }
  public IEnumerable<string>? Labels { get; set; }
}
