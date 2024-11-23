using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketApp.Migrations
{
    /// <inheritdoc />
    public partial class add_theater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Theater",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
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
                    IsCCMS = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theater", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Theater");
        }
    }
}
