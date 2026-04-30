using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ProductManagement_V2.Domain.Entities
{

        public class UserWithRoleView
        {
            public string Id { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string? Email { get; set; } 
            public string? NormalizedEmail { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
            public string? RoleName { get; set; }
            public string? NormalizedRoleName { get; set; }
        }
    
}
