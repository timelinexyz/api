using Application.Behaviors;
using Application.Interfaces;
using Application.Txns.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
  public static void AddApplication(this IServiceCollection services)
  {
    services.AddMediatR(config =>
    {
      config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
      config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    services.AddScoped<IPnl, PnlCalculator>();
  }
}
