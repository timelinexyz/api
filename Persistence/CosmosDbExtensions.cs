using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos;

namespace Persistence;

internal static class CosmosDbExtensions
{
  private const string DATABASE = "pnl";
  private const string TXNS = "txns";

  public static async Task<List<TEntity>> Execute<TEntity>(this FeedIterator<TEntity> iterator)
  {
    var entities = new List<TEntity>();

    using (FeedIterator<TEntity> setIterator = iterator)
    {
      while (setIterator.HasMoreResults)
      {
        foreach (var entity in await setIterator.ReadNextAsync())
        {
          entities.Add(entity);
        }
      }
    }

    return entities;
  }

  public static async Task<TEntity> ExecuteSingle<TEntity>(this FeedIterator<TEntity> iterator)
  {
    var entities = await Execute(iterator);

    if (entities.Count > 1)
    {
      throw new InvalidOperationException("Expected single record but multiple found.");
    }

    return entities.FirstOrDefault();
  }

  public static async Task<List<TEntity>> Execute<TEntity>(this IQueryable<TEntity> query)
  {
    return await Execute(query.ToFeedIterator());
  }

  public static async Task<TEntity> ExecuteSingle<TEntity>(this IQueryable<TEntity> query)
  {
    var entities = await Execute(query);

    if (entities.Count > 1)
    {
      throw new InvalidOperationException("Expected single record but multiple found.");
    }

    return entities.FirstOrDefault();
  }

  public static Container GetTxns(this CosmosClient client) => client.GetContainer(DATABASE, TXNS);
}
