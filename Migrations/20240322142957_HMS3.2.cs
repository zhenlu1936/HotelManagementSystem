using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class HMS32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillRoom_Bills_room_billbill_id",
                table: "BillRoom");

            migrationBuilder.RenameColumn(
                name: "room_billbill_id",
                table: "BillRoom",
                newName: "billsbill_id");

            migrationBuilder.AddForeignKey(
                name: "FK_BillRoom_Bills_billsbill_id",
                table: "BillRoom",
                column: "billsbill_id",
                principalTable: "Bills",
                principalColumn: "bill_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillRoom_Bills_billsbill_id",
                table: "BillRoom");

            migrationBuilder.RenameColumn(
                name: "billsbill_id",
                table: "BillRoom",
                newName: "room_billbill_id");

            migrationBuilder.AddForeignKey(
                name: "FK_BillRoom_Bills_room_billbill_id",
                table: "BillRoom",
                column: "room_billbill_id",
                principalTable: "Bills",
                principalColumn: "bill_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
