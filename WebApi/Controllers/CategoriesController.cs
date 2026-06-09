using Application.Features.Shop.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер управления категориями товаров
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить категорию товара по Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CategoryByIdDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryByIdDto>> GetCategoryById(Guid id, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken));
}
