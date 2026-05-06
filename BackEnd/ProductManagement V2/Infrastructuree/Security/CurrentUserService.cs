using ProductManagement_V2.Application.Common.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductManagement_V2.Infrastructuree.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

        public string? UserId => IsAuthenticated
            ? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User?.FindFirst("sub")?.Value
            : null;

        public string DisplayName => IsAuthenticated
            ? User?.FindFirst("fullName")?.Value
                ?? User?.FindFirst(ClaimTypes.Name)?.Value
                ?? User?.FindFirst(ClaimTypes.Email)?.Value
                ?? UserId
                ?? "System"
            : "System";

        public string? RoleName => IsAuthenticated
            ? User?.FindFirst(ClaimTypes.Role)?.Value
                ?? User?.FindFirst("role")?.Value
            : null;
    }
}
