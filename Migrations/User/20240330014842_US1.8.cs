using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementSystem.Migrations.User
{
    /// <inheritdoc />
    public partial class US18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41b177f5-a8fc-44d9-924a-72ffa7fcae9a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "748c0664-4769-4736-b83b-d6e485fd11d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4ab440a-ad05-45c1-b932-6ddc06613d66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b16d2ed1-a53b-4520-a667-e293c87c16c6");

            migrationBuilder.AddColumn<string>(
                name: "stuff_role",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "经理" },
                    { 2, "管理员" },
                    { 3, "后端" },
                    { 4, "前端" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropColumn(
                name: "stuff_role",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "41b177f5-a8fc-44d9-924a-72ffa7fcae9a", null, "BackStuff", "BACK" },
                    { "748c0664-4769-4736-b83b-d6e485fd11d1", null, "Admin", "ADMIN" },
                    { "a4ab440a-ad05-45c1-b932-6ddc06613d66", null, "FrontStuff", "FRONT" },
                    { "b16d2ed1-a53b-4520-a667-e293c87c16c6", null, "Manager", "MANAGER" }
                });
        }
    }
}
