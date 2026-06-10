using Domain.Repositories;
using MediatR;

namespace Application.Identity.Permissions.Queries.GetPermissions;

public class GetPermissionsQueryHandler(IPermissionRepository permissionRepository)
    : IRequestHandler<GetPermissionsQuery, List<PermissionListDto>>
{
    public async Task<List<PermissionListDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await permissionRepository.GetAllAsync(cancellationToken);
        return permissions.Select(p => new PermissionListDto
        {
            Id = p.Id,
            Name = p.Name,
            Code = p.Code,
            Description = p.Description,
            Group = p.Group,
            CreatedAt = p.CreatedAt
        }).ToList();
    }
}
