namespace Application.DTO.Market;

public class CandleStickData(object[] Data)
{
  public DateTimeOffset OpenTime { get; } = DateTimeOffset.FromUnixTimeMilliseconds((long)Data[0]);
  public decimal OpenPrice { get; } = decimal.Parse((string)Data[1]);
  public decimal HighPrice { get; } = decimal.Parse((string)Data[2]);
  public decimal LowPrice { get; } = decimal.Parse((string)Data[3]);
  public decimal ClosePrice { get; } = decimal.Parse((string)Data[4]);
  public decimal Volume { get; } = decimal.Parse((string)Data[5]);
  public DateTimeOffset CloseTime { get; } = DateTimeOffset.FromUnixTimeMilliseconds((long)Data[6]);
}
