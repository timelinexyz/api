using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

//[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class MarketController(IMarket market) : ControllerBase
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="symbols">Comma separated.</param>
  [HttpGet("prices")]
  public async Task<IActionResult> GetPrices([FromQuery] string symbols)
  {
    string[] symbolsArr = symbols.Split(',');

    // TODO: validation

    var result = await market.GetPrices(symbolsArr);

    return Ok(result);
  }

  [HttpGet("candles")]
  public async Task<IActionResult> CandleStickData([FromQuery] string symbol, DateTimeOffset startTime, DateTimeOffset endTime)
  {
    // TODO: validation

    var result = await market.KlineCandlestickData(symbol, startTime, endTime);

    return Ok(result);
  }
}
