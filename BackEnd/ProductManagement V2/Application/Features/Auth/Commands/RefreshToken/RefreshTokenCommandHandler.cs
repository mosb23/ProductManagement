using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductManagement_V2.Application.Common.Auth;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Auth;
using ProductManagement_V2.Domain.Entities;

namespace ProductManagement_V2.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler
            : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var refreshToken = request.Request.RefreshToken;

            var user = _userManager.Users
                .FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user is null)
                return Result<LoginResponse>.Unauthorized("Invalid refresh token.");

            if (!user.IsActive)
                return Result<LoginResponse>.Forbidden("This account is inactive.");

            if (user.RefreshTokenExpiresAt is null || user.RefreshTokenExpiresAt <= DateTime.UtcNow)
                return Result<LoginResponse>.Unauthorized("Refresh token expired.");

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
