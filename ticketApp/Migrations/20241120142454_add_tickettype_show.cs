using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketApp.Migrations
{
    /// <inheritdoc />
    public partial class add_tickettype_show : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketTypeId",
                table: "Shows",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shows_TicketTypeId",
                table: "Shows",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_TicketTypes_TicketTypeId",
                table: "Shows",
                column: "TicketTypeId",
                principalTable: "TicketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_TicketTypes_TicketTypeId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_TicketTypeId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "TicketTypeId",
                table: "Shows");
        }
    }
}
