using Application.Features.Permissions.Queries.GetPermissionById;
using Application.Features.Permissions.Queries.GetPermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// контроллер управление разрешениями
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/[controller]")]
public class PermissionController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получение всего списка разрешений
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "RequireAdminRole")]
    [ProducesResponseType(typeof(List<PermissionListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PermissionListDto>>> GetPermissions(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetPermissionsQuery(), cancellationToken));
    
    /// <summary>
    /// Получение разрешения по Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PermissionListDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PermissionListDto>> GetPermission(Guid id, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetPermissionByIdQuery(id), cancellationToken));
}