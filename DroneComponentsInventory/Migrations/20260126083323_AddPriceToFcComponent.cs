using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DroneComponentsInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToFcComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "fc_components",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "fc_components");
        }
    }
}
