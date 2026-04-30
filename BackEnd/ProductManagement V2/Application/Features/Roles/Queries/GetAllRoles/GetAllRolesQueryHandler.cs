using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Roles;

namespace ProductManagement_V2.Application.Features.Roles.Queries.GetAllRoles
{
    public class GetAllRolesQueryHandler
        : IRequestHandler<GetAllRolesQuery, Result<PaginatedResult<RoleResponse>>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetAllRolesQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<PaginatedResult<RoleResponse>>> Handle(
            GetAllRolesQuery request,
            CancellationToken cancellationToken)
        {
            var query = request.Query;

            var rolesQuery = _roleManager.Roles
                .OrderBy(r => r.Name)
                .AsQueryable();

            var totalCount = await rolesQuery.CountAsync(cancellationToken);

            var roles = await rolesQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    Name = r.Name!
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<RoleResponse>
            {
                Data = roles,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };

            return Result<PaginatedResult<RoleResponse>>.Success(result);
        }
    }
}