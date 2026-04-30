using MediatR;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Roles;

namespace ProductManagement_V2.Application.Features.Roles.Queries.GetAllRoles
{
    public record GetAllRolesQuery(RoleQueryContract Query) : IRequest<Result<PaginatedResult<RoleResponse>>>;
}