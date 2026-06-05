using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById;
/// <summary>
/// Обработчик запроса по получению разрешения по Id
/// </summary>
/// <param name="permissionRepository"></param>
public class GetPermissionByIdQueryHandler(IPermissionRepository permissionRepository)
    : IRequestHandler<GetPermissionByIdQuery, PermissionDetailDto>
{
    public async Task<PermissionDetailDto> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
    {
        var permission = await permissionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (permission is null)
            throw new NotFoundException("Permission not found", request.Id);

        return new PermissionDetailDto
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description,
            Code = permission.Code,
            Group = permission.Group,
            CreatedAt = permission.CreatedAt,
        };
    }
}