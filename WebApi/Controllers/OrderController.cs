using Application.Features.Shop.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
///
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/[controller]")]
public class OrderController(IMediator mediator) : ControllerBase
{

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("id:{guid}")]
    [Authorize]
    [ProducesResponseType(typeof(OrderByIdDto), statusCode: StatusCodes.Status200OK)]
    public async Task<ActionResult<OrderByIdDto>> GetOrderById(Guid id, CancellationToken cancellationToken)
    => Ok(await mediator.Send(new GetOrderByIdQuery(id), cancellationToken));
}
