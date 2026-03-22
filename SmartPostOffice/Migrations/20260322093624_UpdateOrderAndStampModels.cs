using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPostOffice.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderAndStampModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderLinesJson",
                table: "StampOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderLinesJson",
                table: "StampOrders");
        }
    }
}
