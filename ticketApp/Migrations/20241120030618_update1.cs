using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketApp.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Screens_ScreenId",
                table: "Shows");

            migrationBuilder.DropTable(
                name: "ScreenSeats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Screens",
                table: "Screens");

            migrationBuilder.RenameTable(
                name: "Screens",
                newName: "Screen");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Screen",
                table: "Screen",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ScreenSeat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScreenId = table.Column<int>(type: "INTEGER", nullable: false),
                    RowNo = table.Column<int>(type: "INTEGER", nullable: false),
                    ColNo = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCustom = table.Column<bool>(type: "INTEGER", nullable: true),
                    SeatNo = table.Column<string>(type: "TEXT", nullable: true),
                    TicketTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenSeat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenSeat_Screen_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreenSeat_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScreenSeat_ScreenId",
                table: "ScreenSeat",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenSeat_TicketTypeId",
                table: "ScreenSeat",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Screen_ScreenId",
                table: "Shows",
                column: "ScreenId",
                principalTable: "Screen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Screen_ScreenId",
                table: "Shows");

            migrationBuilder.DropTable(
                name: "ScreenSeat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Screen",
                table: "Screen");

            migrationBuilder.RenameTable(
                name: "Screen",
                newName: "Screens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Screens",
                table: "Screens",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ScreenSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScreenId = table.Column<int>(type: "INTEGER", nullable: false),
                    TicketTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ColNo = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCustom = table.Column<bool>(type: "INTEGER", nullable: true),
                    RowNo = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatNo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenSeats_Screens_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreenSeats_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScreenSeats_ScreenId",
                table: "ScreenSeats",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenSeats_TicketTypeId",
                table: "ScreenSeats",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Screens_ScreenId",
                table: "Shows",
                column: "ScreenId",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
