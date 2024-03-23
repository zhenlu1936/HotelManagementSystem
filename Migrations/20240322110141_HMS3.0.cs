using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class HMS30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Bills_Billbill_id",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_Billbill_id",
                table: "Rooms");

            migrationBuilder.CreateTable(
                name: "BillRoom",
                columns: table => new
                {
                    room_billbill_id = table.Column<int>(type: "int", nullable: false),
                    roomsroom_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillRoom", x => new { x.room_billbill_id, x.roomsroom_id });
                    table.ForeignKey(
                        name: "FK_BillRoom_Bills_room_billbill_id",
                        column: x => x.room_billbill_id,
                        principalTable: "Bills",
                        principalColumn: "bill_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillRoom_Rooms_roomsroom_id",
                        column: x => x.roomsroom_id,
                        principalTable: "Rooms",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BillRoom_roomsroom_id",
                table: "BillRoom",
                column: "roomsroom_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillRoom");

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
    }
}
