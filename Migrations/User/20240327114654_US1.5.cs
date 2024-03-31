using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementSystem.Migrations.User
{
    /// <inheritdoc />
    public partial class US15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0883034c-d557-41da-8047-1eec41a2a12b", null, "BackStuff", "BACK" },
                    { "1e9baa39-6626-48de-8395-14d45496dbde", null, "FrontStuff", "FRONT" },
                    { "87e0cd54-aa91-4cde-b364-92f9755c1158", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0883034c-d557-41da-8047-1eec41a2a12b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e9baa39-6626-48de-8395-14d45496dbde");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87e0cd54-aa91-4cde-b364-92f9755c1158");
        }
    }
}
