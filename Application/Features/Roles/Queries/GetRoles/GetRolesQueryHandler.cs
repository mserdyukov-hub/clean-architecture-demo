using Domain.Repositories;
using MediatR;

namespace Application.Features.Roles.Queries.GetRoles;

/// <summary>
/// Обработчик запроса получения всех ролей
/// </summary>
/// <param name="roleRepository"></param>
public class GetRolesQueryHandler(IRoleRepository roleRepository) : IRequestHandler<GetRolesQuery, List<RoleListDto>>
{
    public async Task<List<RoleListDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(cancellationToken);
        return roles.Select(r => new RoleListDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            CreatedAt = r.CreatedAt,
            IsSystem = r.IsSystem
        }).ToList();
    }
}