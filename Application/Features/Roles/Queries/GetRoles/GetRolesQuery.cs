using MediatR;

namespace Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<List<RoleListDto>>;