using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Identity.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler(IRoleRepository roleRepository) : IRequestHandler<GetRoleByIdQuery, RoleDetailDto>
{
    public async Task<RoleDetailDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetDetailsByIdAsync(request.Id, cancellationToken);
        if (role is null)
            throw new NotFoundException("Role not found", request.Id);

        return new RoleDetailDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsSystem = role.IsSystem,
            CreatedAt = role.CreatedAt,
            Permissions = role.RolePermissions.Select(rp => rp.Permission.Name).ToList()
        };
    }
}
