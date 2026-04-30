using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Users;
using ProductManagement_V2.Infrastructuree;

namespace ProductManagement_V2.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQuery, Result<PaginatedResult<UserResponse>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllUsersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PaginatedResult<UserResponse>>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var query = request.Query;

            var usersQuery = _context.UsersWithRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.FullName))
            {
                var fullName = query.FullName.Trim().ToLower();
                usersQuery = usersQuery.Where(u => u.FullName.ToLower().Contains(fullName));
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                var email = query.Email.Trim().ToUpper();
                usersQuery = usersQuery.Where(u =>
                    u.NormalizedEmail != null && u.NormalizedEmail.Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                var role = query.Role.Trim().ToUpper();
                usersQuery = usersQuery.Where(u =>
                    u.NormalizedRoleName != null && u.NormalizedRoleName == role);
            }

            if (query.IsActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == query.IsActive.Value);
            }

            usersQuery = usersQuery.OrderBy(u => u.FullName);

            var totalCount = await usersQuery.CountAsync(cancellationToken);

            var data = await usersQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email ?? string.Empty,
                    Role = u.RoleName ?? string.Empty,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<UserResponse>
            {
                Data = data,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };

            return Result<PaginatedResult<UserResponse>>.Success(result);
        }
    }
}