using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class seedroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
                         INSERT INTO AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
                         VALUES('1', 'noob', 'NOOB', '{DateTime.Now}' ),
                         ('2', 'elite', 'ELITE', '{DateTime.Now}' ),
                         ('3', 'admin', 'ADMIN', '{DateTime.Now}' )
 ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
