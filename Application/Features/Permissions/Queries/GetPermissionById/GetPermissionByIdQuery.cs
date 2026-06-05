using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById;

public record GetPermissionByIdQuery(Guid Id) : IRequest<PermissionDetailDto>;
