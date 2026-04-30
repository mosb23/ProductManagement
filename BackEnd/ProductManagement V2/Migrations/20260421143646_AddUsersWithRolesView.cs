using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagement_V2.Migrations
{
    public partial class AddUsersWithRolesView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.View_UsersWithRoles', 'V') IS NOT NULL
                    DROP VIEW dbo.View_UsersWithRoles;
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW dbo.View_UsersWithRoles AS
                SELECT
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.NormalizedEmail,
                    u.IsActive,
                    u.CreatedAt,
                    r.Name AS RoleName,
                    r.NormalizedName AS NormalizedRoleName
                FROM AspNetUsers u
                LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('dbo.View_UsersWithRoles', 'V') IS NOT NULL
                    DROP VIEW dbo.View_UsersWithRoles;
            ");
        }
    }
}