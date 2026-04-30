using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductManagement_V2.Application.Common.Constants;
using System.Security.Claims;

namespace ProductManagement_V2.Infrastructuree.Seeders
{
    public static class RolesAndClaimsSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var rolesWithClaims = new Dictionary<string, List<string>>
            {
                [AppRoles.ProjectManager] = new()
                {
                    AppClaims.ProductsView,
                    AppClaims.ProductsCreate,
                    AppClaims.ProductsDelete,
                    AppClaims.ProductsChangeStatus,
                    AppClaims.UsersView,
                    AppClaims.UsersCreate,
                    AppClaims.StatisticsView,
                    AppClaims.ProductStatusHistoriesView,
                    AppClaims.RolesView
                },
                [AppRoles.Supervisor] = new()
                {
                    AppClaims.ProductsView,
                    AppClaims.ProductsCreate,
                    AppClaims.ProductsChangeStatus,
                    AppClaims.StatisticsView,
                    AppClaims.RolesView
                },
                [AppRoles.WarehouseManager] = new()
                {
                    AppClaims.ProductsView,
                    AppClaims.ProductsCreate,
                    AppClaims.ProductsChangeStatus,
                    AppClaims.RolesView
                }
            };

            foreach (var roleEntry in rolesWithClaims)
            {
                var roleName = roleEntry.Key;
                var claims = roleEntry.Value;

                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var createRoleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!createRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", createRoleResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Failed to create role '{roleName}': {errors}");
                    }
                }

                var role = await roleManager.FindByNameAsync(roleName);
                if (role is null)
                    throw new InvalidOperationException($"Role '{roleName}' was not found after creation.");

                var existingClaims = await roleManager.GetClaimsAsync(role);

                foreach (var claimValue in claims)
                {
                    var alreadyExists = existingClaims.Any(c =>
                        c.Type == "permission" && c.Value == claimValue);

                    if (alreadyExists)
                        continue;

                    var addClaimResult = await roleManager.AddClaimAsync(
                        role,
                        new Claim("permission", claimValue));

                    if (!addClaimResult.Succeeded)
                    {
                        var errors = string.Join(", ", addClaimResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException(
                            $"Failed to add claim '{claimValue}' to role '{roleName}': {errors}");
                    }
                }
            }
        }
    }
}



