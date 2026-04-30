using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Roles;

namespace ProductManagement_V2.Application.Features.Roles.Queries.GetRoleClaims
{
    public record GetRoleClaimsQuery(string Id) : IRequest<Result<List<RoleClaimResponse>>>;
}