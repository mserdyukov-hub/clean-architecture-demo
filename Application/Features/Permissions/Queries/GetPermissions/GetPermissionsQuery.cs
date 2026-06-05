using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissions;

public record GetPermissionsQuery : IRequest<List<PermissionListDto>>;