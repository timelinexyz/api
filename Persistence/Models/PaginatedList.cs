using Domain.Interfaces;
using Microsoft.Azure.Cosmos.Linq;

namespace Persistence.Models;

internal class PaginatedList<TEntity> : IPaginatedList<TEntity>
{
  public IReadOnlyCollection<TEntity> Records { get; }
  public int CurrentPage { get; }
  public int TotalPages { get; }
  public int TotalRecords { get; }
  public bool HasPreviousPage => CurrentPage > 1;
  public bool HasNextPage => CurrentPage < TotalPages;

  private PaginatedList(List<TEntity> entities, int totalCount, int pageNumber, int pageSize)
  {
    CurrentPage = pageNumber;
    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    TotalRecords = totalCount;
    Records = entities;
  }

  public static async Task<IPaginatedList<TEntity>> CreateAsync(IQueryable<TEntity> filteredQuery, int pageNumber, int pageSize)
  {
    var count = await filteredQuery.CountAsync();
    var entities = await filteredQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).Execute();

    return new PaginatedList<TEntity>(entities, count, pageNumber, pageSize);
  }

  // TODO: temporary while there is no DB
  public static IPaginatedList<TEntity> Create(IEnumerable<TEntity> filteredQuery, int pageNumber, int pageSize)
  {
    var count = filteredQuery.Count();
    var records = filteredQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    return new PaginatedList<TEntity>(records, count, pageNumber, pageSize);
  }
}
