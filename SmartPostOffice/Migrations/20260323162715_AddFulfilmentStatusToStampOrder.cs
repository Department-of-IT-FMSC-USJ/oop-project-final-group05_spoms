using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPostOffice.Migrations
{
    /// <inheritdoc />
    public partial class AddFulfilmentStatusToStampOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FulfilmentStatus",
                table: "StampOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StampTrackingHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StampOrderId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByOfficerId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StampTrackingHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StampTrackingHistories_StampOrders_StampOrderId",
                        column: x => x.StampOrderId,
                        principalTable: "StampOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StampTrackingHistories_StampOrderId",
                table: "StampTrackingHistories",
                column: "StampOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StampTrackingHistories");

            migrationBuilder.DropColumn(
                name: "FulfilmentStatus",
                table: "StampOrders");
        }
    }
}
