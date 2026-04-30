using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Users;
using ProductManagement_V2.Domain.Entities;

namespace ProductManagement_V2.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<UserResponse>> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var input = request.Request;

            var roleExists = await _roleManager.RoleExistsAsync(input.Role);
            if (!roleExists)
                return Result<UserResponse>.BadRequest("Selected role does not exist.");

            var existingUser = await _userManager.FindByEmailAsync(input.Email);
            if (existingUser is not null)
                return Result<UserResponse>.Conflict("Email already exists.");

            var user = new ApplicationUser(input.FullName, input.Email);

            var createResult = await _userManager.CreateAsync(user, input.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description).ToList();
                return Result<UserResponse>.BadRequest(errors);
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, input.Role);
            if (!addToRoleResult.Succeeded)
            {
                var errors = addToRoleResult.Errors.Select(e => e.Description).ToList();
                return Result<UserResponse>.BadRequest(errors);
            }

            var response = new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Role = input.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return Result<UserResponse>.Success(response);
        }
    }
}