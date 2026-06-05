using MediatR;

namespace Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id): IRequest<RoleDetailDto>;