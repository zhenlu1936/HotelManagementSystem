using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementSystem.Migrations.User
{
    /// <inheritdoc />
    public partial class US17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57c3bbfd-6d39-4283-995b-d31d32a9e57a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dbf57b96-bb4d-4204-a3db-29bb5be2c985");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3815c3a-3559-43d7-80a6-9683860e4185");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5abcb82-be1d-4b7b-8083-afd6aefb204d");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "57c3bbfd-6d39-4283-995b-d31d32a9e57a", null, "BackStuff", "BACK" },
                    { "dbf57b96-bb4d-4204-a3db-29bb5be2c985", null, "Admin", "ADMIN" },
                    { "e3815c3a-3559-43d7-80a6-9683860e4185", null, "Manager", "MANAGER" },
                    { "e5abcb82-be1d-4b7b-8083-afd6aefb204d", null, "FrontStuff", "FRONT" }
                });
        }
    }
}
