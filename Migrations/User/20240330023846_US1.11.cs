using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementSystem.Migrations.User
{
    /// <inheritdoc />
    public partial class US111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stuff_role",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "StuffRoles",
                newName: "role_name");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "StuffRoles",
                newName: "role_id");

            migrationBuilder.AddColumn<int>(
                name: "Rolerole_id",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rolerole_id",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "role_name",
                table: "StuffRoles",
                newName: "RoleName");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "StuffRoles",
                newName: "RoleId");

            migrationBuilder.AddColumn<string>(
                name: "stuff_role",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
