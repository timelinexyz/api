﻿using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
  public static void AddInfrastructure(this IServiceCollection services)
  {
    services.AddHttpClient<IMarket, Binance.Binance>();
    services.AddSingleton(Koinly.Extensions.GetKoinlyTxns());
  }
}
