using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductManagement_V2.Application.Common.Auth;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Auth;
using ProductManagement_V2.Domain.Entities;

namespace ProductManagement_V2.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var input = request.Request;

            var user = await _userManager.FindByEmailAsync(input.Email);

            // Same message for wrong email and wrong password
            const string invalidCredentialsMessage = "Invalid email or password.";

            if (user is null)
                return Result<LoginResponse>.Unauthorized(invalidCredentialsMessage);

            var passwordValid = await _userManager.CheckPasswordAsync(user, input.Password);
            if (!passwordValid)
                return Result<LoginResponse>.Unauthorized(invalidCredentialsMessage);

            if (!user.IsActive)
                return Result<LoginResponse>.Forbidden("This account is inactive.");

            var tokenResult = await _jwtTokenService.GenerateTokenAsync(user);
            user.SetRefreshToken(tokenResult.RefreshToken, tokenResult.RefreshTokenExpiresAt);
            await _userManager.UpdateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var response = new LoginResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? string.Empty,
                Claims = tokenResult.Claims,
                Token = tokenResult.Token,
                ExpiresAt = tokenResult.ExpiresAt,
                RefreshToken = tokenResult.RefreshToken,
                RefreshTokenExpiresAt = tokenResult.RefreshTokenExpiresAt
            };

            return Result<LoginResponse>.Success(response);
        }
    }
}