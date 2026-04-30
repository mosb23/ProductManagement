using Microsoft.AspNetCore.Identity;

namespace ProductManagement_V2.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiresAt { get; private set; }

        private ApplicationUser()
        {
        }

        public ApplicationUser(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
            UserName = email;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateFullName(string fullName)
        {
            FullName = fullName;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetRefreshToken(string refreshToken, DateTime expiresAt)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiresAt = expiresAt;
        }

        public void ClearRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiresAt = null;
        }
    }
}