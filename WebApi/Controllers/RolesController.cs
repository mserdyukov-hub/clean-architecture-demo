using Application.Identity.Roles.Queries.GetRoleById;
using Application.Identity.Roles.Queries.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер управления ролями
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/[controller]")]
public class RolesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получение списка всех ролей
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(typeof(List<RoleListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RoleListDto>>> GetRoles(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetRolesQuery(), cancellationToken));

    /// <summary>
    /// Получение роли и доступов по Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(typeof(RoleDetailDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleDetailDto>> GetRole(Guid id, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetRoleByIdQuery(id), cancellationToken));
}
