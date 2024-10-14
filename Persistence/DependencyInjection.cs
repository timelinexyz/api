using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
  public static void AddPersistence(this IServiceCollection services)
  {
    services.AddSingleton(Koinly.Extensions.GetKoinlyTxns());
    services.AddScoped<ITxnRepository, TxnRepository>();
  }
}
