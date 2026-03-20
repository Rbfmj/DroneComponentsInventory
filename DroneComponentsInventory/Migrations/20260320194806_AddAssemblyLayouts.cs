using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DroneComponentsInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddAssemblyLayouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assembly_layouts",
                columns: table => new
                {
                    build_id = table.Column<int>(type: "INTEGER", nullable: false),
                    layout_json = table.Column<string>(type: "TEXT", nullable: false),
                    saved_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assembly_layouts", x => x.build_id);
                    table.ForeignKey(
                        name: "fk_assembly_layouts_drone_builds_build_id",
                        column: x => x.build_id,
                        principalTable: "drone_builds",
                        principalColumn: "build_id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assembly_layouts");
        }
    }
}
