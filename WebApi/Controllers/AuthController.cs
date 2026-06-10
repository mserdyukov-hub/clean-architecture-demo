using Application.Identity.Auth.Commands.Login;
using Application.Identity.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер регистрации и аутентификации
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Вход в систему. Возвращает JWT токен
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginCommand request,
        CancellationToken cancellationToken)
        => Ok(await mediator.Send(request, cancellationToken));


    /// <summary>
    /// Регистрация нового пользователя. Возвращает ID созданного пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<ActionResult<Guid>> Register(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
        => Ok(await mediator.Send(command, cancellationToken));
}
