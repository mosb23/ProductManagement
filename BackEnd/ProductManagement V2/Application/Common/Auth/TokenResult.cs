namespace ProductManagement_V2.Application.Common.Auth
{
    public class TokenResult
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public List<string> Claims { get; set; } = new();

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
