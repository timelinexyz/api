using Application.Txns.Commands.DeleteTxns;
using Application.Txns.Commands.UpdateTxn;
using Application.Txns.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

//[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TxnsController(ISender sender) : ControllerBase
{
  [HttpPost("search")]
  public async Task<IActionResult> Search([Required][FromBody] SearchTxnsQuery query)
  {
    var result = await sender.Send(query);

    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    else
    {
      // TODO: exception handling? what if different errors result din different status codes?
      return BadRequest(result.Error);
    }
  }

  [HttpPatch("update")]
  public async Task<IActionResult> Update([Required][FromBody] UpdateTxnCommand updateCommand)
  {
    var result = await sender.Send(updateCommand);

    if (result.IsSuccess)
    {
      return Ok();
    }
    else
    {
      // TODO: exception handling? what if different errors result din different status codes?
      return BadRequest(result.Error);
    }
  }

  [HttpPatch("delete")]
  public async Task<IActionResult> Delete([Required][FromBody] DeleteTxnsCommand deleteCommand)
  {
    var result = await sender.Send(deleteCommand);

    if (result.IsSuccess)
    {
      return Ok();
    }
    else
    {
      // TODO: exception handling? what if different errors result din different status codes?
      return BadRequest(result.Error);
    }
  }
}
