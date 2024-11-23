using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketApp.Migrations
{
    /// <inheritdoc />
    public partial class seatId_fk_on_ticketSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeatId",
                table: "TicketSeat",
                newName: "ScreenSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSeat_ScreenSeatId",
                table: "TicketSeat",
                column: "ScreenSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSeat_ScreenSeat_ScreenSeatId",
                table: "TicketSeat",
                column: "ScreenSeatId",
                principalTable: "ScreenSeat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketSeat_ScreenSeat_ScreenSeatId",
                table: "TicketSeat");

            migrationBuilder.DropIndex(
                name: "IX_TicketSeat_ScreenSeatId",
                table: "TicketSeat");

            migrationBuilder.RenameColumn(
                name: "ScreenSeatId",
                table: "TicketSeat",
                newName: "SeatId");
        }
    }
}
