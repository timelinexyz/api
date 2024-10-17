namespace Core.Filters;

public class PaginationFilter
{
  public required int PageNumber { get; set; }
  public required int PageSize { get; set; }
}
