using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class seedadmindata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
                         INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, SecurityStamp, ConcurrencyStamp, 
                            AccessFailedCount,
                            EmailConfirmed,
                            PhoneNumberConfirmed,
                            LockoutEnabled,
                            TwoFactorEnabled)
                         VALUES('1', 'admin', 'ADMIN', 'admin@example.com', 'ADMIN@EXAMPLE.COM', '{DateTime.Now}', '{DateTime.Now}', 
                            0,
                            0,
                            0,
                            0,
                            0)
            ");

            migrationBuilder.Sql(@$"
                         INSERT INTO AspNetUserRoles(UserId, RoleId)
                         VALUES('1', '3')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
