namespace Domain.Interfaces;

public interface IPaginatedList<TRecord>
{
  IReadOnlyCollection<TRecord> Records { get; }
  int CurrentPage { get; }
  int TotalPages { get; }
  int TotalRecords { get; }
  bool HasPreviousPage { get; }
  bool HasNextPage { get; }
}
