using Core.Filters;
namespace Domain.Payloads;

public class CategoryFilter
{
  public StringFilter? Type { get; set; }
  public StringFilter? Labels { get; set; }
  public bool IsValid() => Type is not null || Labels is not null;
}
