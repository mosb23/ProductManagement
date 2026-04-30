using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Users;
using ProductManagement_V2.Domain.Entities;

namespace ProductManagement_V2.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<UserResponse>> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user is null)
                return Result<UserResponse>.NotFound("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            var response = new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? string.Empty,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return Result<UserResponse>.Success(response);
        }
    }
}