using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPostOffice.Migrations
{
    /// <inheritdoc />
    public partial class AddDayBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BalanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    SystemCashTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SystemOnlineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SystemTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhysicalCashCounted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discrepancy = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ClosedByOfficerId = table.Column<int>(type: "int", nullable: false),
                    TotalEntries = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayBalances", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayBalances");
        }
    }
}
