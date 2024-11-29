using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketApp.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsInternational = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsThreeD = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Screen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Row = table.Column<int>(type: "INTEGER", nullable: false),
                    Column = table.Column<int>(type: "INTEGER", nullable: false),
                    Seats = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theater",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    VatNo = table.Column<string>(type: "TEXT", nullable: false),
                    IsVat = table.Column<bool>(type: "INTEGER", nullable: false),
                    ThreeDCharge = table.Column<int>(type: "INTEGER", nullable: false),
                    FDF = table.Column<decimal>(type: "TEXT", nullable: false),
                    LocalTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    LocalTaxInternational = table.Column<decimal>(type: "TEXT", nullable: false),
                    BoxOfficeTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    EntertainmentTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    ConvinienceCharge = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsCCMS = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogoPath = table.Column<string>(type: "TEXT", nullable: true),
                    QRCode = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableTaxRegistration = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theater", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TotalAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScreenId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shows_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shows_Screen_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScreenSeat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScreenId = table.Column<int>(type: "INTEGER", nullable: false),
                    RowNo = table.Column<int>(type: "INTEGER", nullable: false),
                    RowName = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShowId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ticket_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketSeat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScreenSeatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSeat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketSeat_ScreenSeat_ScreenSeatId",
                        column: x => x.ScreenSeatId,
                        principalTable: "ScreenSeat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketSeat_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
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

            migrationBuilder.CreateIndex(
                name: "IX_Shows_MovieId",
                table: "Shows",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ScreenId",
                table: "Shows",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ShowId",
                table: "Ticket",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TransactionId",
                table: "Ticket",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSeat_ScreenSeatId",
                table: "TicketSeat",
                column: "ScreenSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSeat_TicketId",
                table: "TicketSeat",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Theater");

            migrationBuilder.DropTable(
                name: "TicketSeat");

            migrationBuilder.DropTable(
                name: "ScreenSeat");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropTable(
                name: "Shows");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Screen");
        }
    }
}
