using Application.Shop.Products.Queries;
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
public class ProductController(IMediator mediator) : ControllerBase
{

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("id:{guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ProductByIdDto), statusCode: StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductByIdDto>> GetProductById(Guid id, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetProductByIdQuery(id), cancellationToken));
}
