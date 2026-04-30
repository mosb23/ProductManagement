using ProductManagement_V2.Domain.Entities;

namespace ProductManagement_V2.Application.Common.Auth
{

        public interface IJwtTokenService
        {
            Task<TokenResult> GenerateTokenAsync(ApplicationUser user);
        }
}
