using Core;
using Core.Filters;
using Domain.Enums;
using Domain.Payloads;
using FluentValidation;

namespace Application.Txns.Queries.Search;

public sealed class AmountFilterValidator : AbstractValidator<AmountFilter>
{
  private static readonly HashSet<Operator> _validEnumFilterOperators = [Operator.Equals, Operator.In];

  public AmountFilterValidator()
  {
    RuleFor(f => f)
      .Must(f => f.IsValid())
      .WithMessage("{PropertyName} must have at least one of Currency/Wallet filters set.");

    // Currency
    When(f => f.Currency is not null, () =>
    {
      RuleFor(f => f.Currency)
        .Must(f => f!.IsValid())
        .WithMessage("{PropertyName} must have at least one of Type/Symbol filters set.");

      // Type
      When(f => f.Currency!.Type is not null, () =>
      {
        RuleFor(f => f.Currency!.Type!.Operator)
          .Must(o => _validEnumFilterOperators.Contains(o))
          .WithMessage($"Invalid Operator. Allowed values: {string.Join(", ", _validEnumFilterOperators.Select(o => o.ToString()))}");

        RuleFor(f => f.Currency!.Type!.Values)
          .Must(v => !v.IsNullOrEmpty() && v.All(l => l != CurrencyType.None))
          .WithMessage("Values must contain at least a single currency type. Allowed values: Crypto, Stablecoin, Fiat")
          .When(f => f.Currency!.Type!.Operator is Operator.In);

        RuleFor(f => f.Currency!.Type!.Values)
          .Must(v => !v.IsNullOrEmpty() && v.Count == 1 && v.First() != CurrencyType.None)
          .WithMessage("Values must contain a single currency type. Allowed values: Crypto, Stablecoin, Fiat")
          .When(f => f.Currency!.Type!.Operator is Operator.Equals);
      });

      // Symbol
      RuleFor(f => f.Currency!.Symbol)
        .SetValidator(new StringFilterValidator()!)
        .When(f => f.Currency!.Symbol is not null);
    });

    // Wallet
    When(f => f.Wallet is not null, () =>
    {
      RuleFor(f => f.Wallet)
        .Must(f => f!.IsValid())
        .WithMessage("{PropertyName} must have at least one of Type/Name filters set.");

      // Name
      RuleFor(f => f.Wallet!.Name)
        .SetValidator(new StringFilterValidator()!)
        .When(f => f.Wallet!.Name is not null);

      // Type
      When(f => f.Wallet!.Type is not null, () =>
      {
        RuleFor(f => f.Wallet!.Type!.Operator)
          .Must(o => _validEnumFilterOperators.Contains(o))
          .WithMessage($"Invalid Operator. Allowed values: {string.Join(", ", _validEnumFilterOperators.Select(o => o.ToString()))}");

        RuleFor(f => f.Wallet!.Type!.Values)
          .Must(v => !v.IsNullOrEmpty() && v.All(l => l != WalletType.None))
          .WithMessage("Values must contain at least a single currency type. Allowed values: Unknown, Exchange, SelfCustody")
          .When(f => f.Wallet!.Type!.Operator is Operator.In);

        RuleFor(f => f.Wallet!.Type!.Values)
          .Must(v => !v.IsNullOrEmpty() && v.Count == 1 && v.First() != WalletType.None)
          .WithMessage("Values must contain a single currency type. Allowed values: Unknown, Exchange, SelfCustody")
          .When(f => f.Wallet!.Type!.Operator is Operator.Equals);
      });
    });
  }
}
