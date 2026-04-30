using ProductManagement_V2.Application.DTOs;

namespace ProductManagement_V2.Application.Contract.Users
{
    public class UserQueryContract : PaginationBaseDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
    }
}