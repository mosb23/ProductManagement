using Microsoft.AspNetCore.Identity;
using ProductManagement_V2.Application.Common.Constants;
using ProductManagement_V2.Domain.Entities;

namespace ProductManagement_V2.Infrastructuree.Seeders
{
    public static class AdminUserSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleExists = await roleManager.RoleExistsAsync(AppRoles.ProjectManager);

            if (!roleExists)
                return;

            var usersInRole = await userManager.GetUsersInRoleAsync(AppRoles.ProjectManager);

            if (usersInRole.Any())
                return;

            var email = "admin@system.com";

            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser(
                    fullName: "System Admin",
                    email: email);

                var createResult = await userManager.CreateAsync(
                    user,
                    "Admin@123");

                if (!createResult.Succeeded)
                    return;
            }

            if (!await userManager.IsInRoleAsync(user, AppRoles.ProjectManager))
            {
                await userManager.AddToRoleAsync(user, AppRoles.ProjectManager);
            }
        }
    }
}