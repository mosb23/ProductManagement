namespace ProductManagement_V2.Application.Common.Auth
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string DisplayName { get; }
        string? RoleName { get; }
        bool IsAuthenticated { get; }
    }
}
