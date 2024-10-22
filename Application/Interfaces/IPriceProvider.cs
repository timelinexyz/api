namespace Application.Interfaces;

public interface IPriceProvider
{
  Task<string> GetPrices(string symbols);
}
