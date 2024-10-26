using Core.Filters;

namespace Domain.Payloads;

public class CategoryFilter
{
  public StringFilter? Type { get; set; }
  public StringFilter? Labels { get; set; }
}
