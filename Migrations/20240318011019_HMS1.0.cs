using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class HMS10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Billbill_id",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Billbill_id",
                table: "Rooms",
                column: "Billbill_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Bills_Billbill_id",
                table: "Rooms",
                column: "Billbill_id",
                principalTable: "Bills",
                principalColumn: "bill_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Bills_Billbill_id",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_Billbill_id",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Billbill_id",
                table: "Rooms");
        }
    }
}
