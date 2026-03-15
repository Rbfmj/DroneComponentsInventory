using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DroneComponentsInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddDroneBuildsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "telemetry_support",
                table: "receiver_components",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "failsafe_support",
                table: "receiver_components",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "telemetry_support",
                table: "radio_controller_components",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "drone_builds",
                columns: table => new
                {
                    build_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    frame_id = table.Column<int>(type: "INTEGER", nullable: true),
                    motor_id = table.Column<int>(type: "INTEGER", nullable: true),
                    propeller_id = table.Column<int>(type: "INTEGER", nullable: true),
                    esc_id = table.Column<int>(type: "INTEGER", nullable: true),
                    battery_id = table.Column<int>(type: "INTEGER", nullable: true),
                    fc_id = table.Column<int>(type: "INTEGER", nullable: true),
                    camera_id = table.Column<int>(type: "INTEGER", nullable: true),
                    vtx_id = table.Column<int>(type: "INTEGER", nullable: true),
                    video_antenna_id = table.Column<int>(type: "INTEGER", nullable: true),
                    receiver_id = table.Column<int>(type: "INTEGER", nullable: true),
                    receiver_antenna_id = table.Column<int>(type: "INTEGER", nullable: true),
                    radio_controller_id = table.Column<int>(type: "INTEGER", nullable: true),
                    fpv_goggles_id = table.Column<int>(type: "INTEGER", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drone_builds", x => x.build_id);
                    table.ForeignKey(
                        name: "fk_drone_builds_battery_components_battery_id",
                        column: x => x.battery_id,
                        principalTable: "battery_components",
                        principalColumn: "battery_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_esc_components_esc_id",
                        column: x => x.esc_id,
                        principalTable: "esc_components",
                        principalColumn: "esc_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_fc_components_fc_id",
                        column: x => x.fc_id,
                        principalTable: "fc_components",
                        principalColumn: "fc_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_fpv_camera_components_camera_id",
                        column: x => x.camera_id,
                        principalTable: "fpv_camera_components",
                        principalColumn: "camera_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_fpv_goggles_components_fpv_goggles_id",
                        column: x => x.fpv_goggles_id,
                        principalTable: "fpv_goggles_components",
                        principalColumn: "fpv_goggles_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_frame_components_frame_id",
                        column: x => x.frame_id,
                        principalTable: "frame_components",
                        principalColumn: "frame_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_motors_components_motor_id",
                        column: x => x.motor_id,
                        principalTable: "motors_components",
                        principalColumn: "motor_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_propellers_components_propeller_id",
                        column: x => x.propeller_id,
                        principalTable: "propellers_components",
                        principalColumn: "propeller_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_radio_controller_components_radio_controller_id",
                        column: x => x.radio_controller_id,
                        principalTable: "radio_controller_components",
                        principalColumn: "radio_controller_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_receiver_antenna_components_receiver_antenna_id",
                        column: x => x.receiver_antenna_id,
                        principalTable: "receiver_antenna_components",
                        principalColumn: "receiver_antenna_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_receiver_components_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "receiver_components",
                        principalColumn: "receiver_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_video_antenna_components_video_antenna_id",
                        column: x => x.video_antenna_id,
                        principalTable: "video_antenna_components",
                        principalColumn: "antenna_id");
                    table.ForeignKey(
                        name: "fk_drone_builds_video_transmitter_components_vtx_id",
                        column: x => x.vtx_id,
                        principalTable: "video_transmitter_components",
                        principalColumn: "vtx_id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_battery_id",
                table: "drone_builds",
                column: "battery_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_camera_id",
                table: "drone_builds",
                column: "camera_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_esc_id",
                table: "drone_builds",
                column: "esc_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_fc_id",
                table: "drone_builds",
                column: "fc_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_fpv_goggles_id",
                table: "drone_builds",
                column: "fpv_goggles_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_frame_id",
                table: "drone_builds",
                column: "frame_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_motor_id",
                table: "drone_builds",
                column: "motor_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_propeller_id",
                table: "drone_builds",
                column: "propeller_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_radio_controller_id",
                table: "drone_builds",
                column: "radio_controller_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_receiver_antenna_id",
                table: "drone_builds",
                column: "receiver_antenna_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_receiver_id",
                table: "drone_builds",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_video_antenna_id",
                table: "drone_builds",
                column: "video_antenna_id");

            migrationBuilder.CreateIndex(
                name: "ix_drone_builds_vtx_id",
                table: "drone_builds",
                column: "vtx_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drone_builds");

            migrationBuilder.AlterColumn<bool>(
                name: "telemetry_support",
                table: "receiver_components",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "failsafe_support",
                table: "receiver_components",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "telemetry_support",
                table: "radio_controller_components",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");
        }
    }
}
