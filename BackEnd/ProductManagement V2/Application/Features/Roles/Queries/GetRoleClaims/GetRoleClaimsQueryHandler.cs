using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Roles;


namespace ProductManagement_V2.Application.Features.Roles.Queries.GetRoleClaims
{
    public class GetRoleClaimsQueryHandler : IRequestHandler<GetRoleClaimsQuery, Result<List<RoleClaimResponse>>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetRoleClaimsQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<List<RoleClaimResponse>>> Handle(
            GetRoleClaimsQuery request,
            CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);

            if (role is null)
                return Result<List<RoleClaimResponse>>.NotFound("Role not found");

            var claims = await _roleManager.GetClaimsAsync(role);

            var result = claims
                .Select(c => new RoleClaimResponse
                {
                    Type = c.Type,
                    Value = c.Value
                })
                .ToList();

            return Result<List<RoleClaimResponse>>.Success(result);
        }
    }
}