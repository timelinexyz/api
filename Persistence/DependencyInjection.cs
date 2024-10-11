using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Persistence.Models;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
  public static void AddPersistence(this IServiceCollection services)
  {
    services.AddSingleton(ReadAllKoinlyTransactionFiles());
    services.AddScoped<ITxnRepository, TxnRepository>();
  }

  private static KoinlyTxn[] ReadAllKoinlyTransactionFiles()
  {
    var pages = new List<KoinlyTxnPage>();

    foreach (string file in Directory.GetFiles("P:\\Portfolio Tracker\\koinly - all TXs", "*.json"))
    {
      string contents = File.ReadAllText(file);
      var page = JsonConvert.DeserializeObject<KoinlyTxnPage>(contents);
      pages.Add(page);
    }

    return pages
      .SelectMany(p => p.transactions)
      .OrderByDescending(t => t.date)
      .ToArray();
  }
}
