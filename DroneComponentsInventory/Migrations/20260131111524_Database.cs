using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DroneComponentsInventory.Migrations
{
    /// <inheritdoc />
    public partial class Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "battery_components",
                columns: table => new
                {
                    battery_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    cell_count_s = table.Column<int>(type: "INTEGER", nullable: true),
                    nominal_voltage_v = table.Column<double>(type: "REAL", nullable: true),
                    capacity_mah = table.Column<int>(type: "INTEGER", nullable: true),
                    discharge_rate_c = table.Column<double>(type: "REAL", nullable: true),
                    burst_rate_c = table.Column<double>(type: "REAL", nullable: true),
                    discharge_connector = table.Column<string>(type: "TEXT", nullable: true),
                    balance_connector = table.Column<string>(type: "TEXT", nullable: true),
                    chemistry = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    length_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    width_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    height_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_battery_components", x => x.battery_id);
                });

            migrationBuilder.CreateTable(
                name: "esc_components",
                columns: table => new
                {
                    esc_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    esc_type = table.Column<string>(type: "TEXT", nullable: true),
                    continuous_current_a = table.Column<double>(type: "REAL", nullable: true),
                    voltage_input_s = table.Column<int>(type: "INTEGER", nullable: true),
                    mount_pattern_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    supported_protocols = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_esc_components", x => x.esc_id);
                });

            migrationBuilder.CreateTable(
                name: "fc_components",
                columns: table => new
                {
                    fc_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    mcu_processor = table.Column<string>(type: "TEXT", nullable: true),
                    imu_gyro = table.Column<string>(type: "TEXT", nullable: true),
                    mount_pattern_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    voltage_input_s = table.Column<string>(type: "TEXT", nullable: true),
                    firmware_support = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fc_components", x => x.fc_id);
                });

            migrationBuilder.CreateTable(
                name: "fpv_camera_components",
                columns: table => new
                {
                    camera_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    type_system = table.Column<string>(type: "TEXT", nullable: true),
                    sensor_size = table.Column<string>(type: "TEXT", nullable: true),
                    resolution_tvl = table.Column<int>(type: "INTEGER", nullable: true),
                    fov_modes = table.Column<string>(type: "TEXT", nullable: true),
                    lens_focal_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    supported_aspect_ratios = table.Column<string>(type: "TEXT", nullable: true),
                    low_light_lux = table.Column<double>(type: "REAL", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    mount_size_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fpv_camera_components", x => x.camera_id);
                });

            migrationBuilder.CreateTable(
                name: "fpv_goggles_components",
                columns: table => new
                {
                    fpv_goggles_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    video_system = table.Column<string>(type: "TEXT", nullable: true),
                    display_type = table.Column<string>(type: "TEXT", nullable: true),
                    screen_size_inch = table.Column<double>(type: "REAL", nullable: true),
                    resolution = table.Column<string>(type: "TEXT", nullable: true),
                    latency_ms = table.Column<int>(type: "INTEGER", nullable: true),
                    receiver_type = table.Column<string>(type: "TEXT", nullable: true),
                    diversity_antennas = table.Column<int>(type: "INTEGER", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    dvr_capability = table.Column<bool>(type: "INTEGER", nullable: true),
                    battery_life_hours = table.Column<double>(type: "REAL", nullable: true),
                    power_input = table.Column<string>(type: "TEXT", nullable: true),
                    ipd_adjustable = table.Column<bool>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fpv_goggles_components", x => x.fpv_goggles_id);
                });

            migrationBuilder.CreateTable(
                name: "frame_components",
                columns: table => new
                {
                    frame_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    wheelbase_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    max_prop_inch = table.Column<double>(type: "REAL", nullable: true),
                    geometry = table.Column<string>(type: "TEXT", nullable: true),
                    material = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    arm_thickness_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    motor_mount_pattern = table.Column<string>(type: "TEXT", nullable: true),
                    fc_mount_pattern = table.Column<string>(type: "TEXT", nullable: true),
                    max_stack_height_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_frame_components", x => x.frame_id);
                });

            migrationBuilder.CreateTable(
                name: "motors_components",
                columns: table => new
                {
                    motor_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    stator_size_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    kv = table.Column<int>(type: "INTEGER", nullable: true),
                    mount_pattern = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    recommended_voltage_s = table.Column<int>(type: "INTEGER", nullable: true),
                    recommended_prop_inch = table.Column<double>(type: "REAL", nullable: true),
                    max_thrust_g = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_motors_components", x => x.motor_id);
                });

            migrationBuilder.CreateTable(
                name: "propellers_components",
                columns: table => new
                {
                    propeller_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    diameter_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    pitch_inch = table.Column<int>(type: "INTEGER", nullable: true),
                    blade_count = table.Column<int>(type: "INTEGER", nullable: true),
                    material = table.Column<string>(type: "TEXT", nullable: true),
                    shaft_diameter_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    rotation_direction = table.Column<string>(type: "TEXT", nullable: true),
                    recommended_motor_size = table.Column<string>(type: "TEXT", nullable: true),
                    recommended_motor_kv = table.Column<int>(type: "INTEGER", nullable: true),
                    frame_class = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    color_options = table.Column<string>(type: "TEXT", nullable: true),
                    included_quantity = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_propellers_components", x => x.propeller_id);
                });

            migrationBuilder.CreateTable(
                name: "radio_controller_components",
                columns: table => new
                {
                    radio_controller_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    controller_style = table.Column<string>(type: "TEXT", nullable: true),
                    frequency_ghz = table.Column<double>(type: "REAL", nullable: true),
                    protocols_supported = table.Column<string>(type: "TEXT", nullable: true),
                    max_channels = table.Column<int>(type: "INTEGER", nullable: true),
                    output_power_mw = table.Column<double>(type: "REAL", nullable: true),
                    telemetry_support = table.Column<bool>(type: "INTEGER", nullable: true),
                    screen_type = table.Column<string>(type: "TEXT", nullable: true),
                    gimbal_type = table.Column<string>(type: "TEXT", nullable: true),
                    battery_type = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    firmware_support = table.Column<string>(type: "TEXT", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_radio_controller_components", x => x.radio_controller_id);
                });

            migrationBuilder.CreateTable(
                name: "receiver_antenna_components",
                columns: table => new
                {
                    receiver_antenna_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    frequency_band = table.Column<string>(type: "TEXT", nullable: true),
                    polarization = table.Column<string>(type: "TEXT", nullable: true),
                    connector = table.Column<string>(type: "TEXT", nullable: true),
                    connector_gender = table.Column<string>(type: "TEXT", nullable: true),
                    gain_dbi = table.Column<double>(type: "REAL", nullable: true),
                    radiation_pattern = table.Column<string>(type: "TEXT", nullable: true),
                    compatible_receivers = table.Column<string>(type: "TEXT", nullable: true),
                    mount_type = table.Column<string>(type: "TEXT", nullable: true),
                    cable_length_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    length_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_receiver_antenna_components", x => x.receiver_antenna_id);
                });

            migrationBuilder.CreateTable(
                name: "receiver_components",
                columns: table => new
                {
                    receiver_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    protocol = table.Column<string>(type: "TEXT", nullable: true),
                    modulation_protocol = table.Column<string>(type: "TEXT", nullable: true),
                    frequency_band = table.Column<string>(type: "TEXT", nullable: true),
                    channels = table.Column<int>(type: "INTEGER", nullable: true),
                    telemetry_support = table.Column<bool>(type: "INTEGER", nullable: true),
                    antenna_count = table.Column<int>(type: "INTEGER", nullable: true),
                    antenna_type = table.Column<string>(type: "TEXT", nullable: true),
                    range_km = table.Column<double>(type: "REAL", nullable: true),
                    voltage_input_v = table.Column<double>(type: "REAL", nullable: true),
                    output_signal = table.Column<string>(type: "TEXT", nullable: true),
                    failsafe_support = table.Column<bool>(type: "INTEGER", nullable: true),
                    binding_method = table.Column<string>(type: "TEXT", nullable: true),
                    intended_use = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    length_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    width_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    height_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    mounting_type = table.Column<string>(type: "TEXT", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_receiver_components", x => x.receiver_id);
                });

            migrationBuilder.CreateTable(
                name: "video_antenna_components",
                columns: table => new
                {
                    antenna_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    antenna_class = table.Column<string>(type: "TEXT", nullable: true),
                    polarization = table.Column<string>(type: "TEXT", nullable: true),
                    connector = table.Column<string>(type: "TEXT", nullable: true),
                    gain_dbi = table.Column<double>(type: "REAL", nullable: true),
                    axial_ratio = table.Column<double>(type: "REAL", nullable: true),
                    radiation_pattern = table.Column<string>(type: "TEXT", nullable: true),
                    operating_frequency_ghz = table.Column<double>(type: "REAL", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_video_antenna_components", x => x.antenna_id);
                });

            migrationBuilder.CreateTable(
                name: "video_transmitter_components",
                columns: table => new
                {
                    vtx_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    type = table.Column<string>(type: "TEXT", nullable: true),
                    max_power_mw = table.Column<double>(type: "REAL", nullable: true),
                    voltage_input_s = table.Column<double>(type: "REAL", nullable: true),
                    mount_pattern_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    control_protocols = table.Column<string>(type: "TEXT", nullable: true),
                    antenna_connector = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_video_transmitter_components", x => x.vtx_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "battery_components");

            migrationBuilder.DropTable(
                name: "esc_components");

            migrationBuilder.DropTable(
                name: "fc_components");

            migrationBuilder.DropTable(
                name: "fpv_camera_components");

            migrationBuilder.DropTable(
                name: "fpv_goggles_components");

            migrationBuilder.DropTable(
                name: "frame_components");

            migrationBuilder.DropTable(
                name: "motors_components");

            migrationBuilder.DropTable(
                name: "propellers_components");

            migrationBuilder.DropTable(
                name: "radio_controller_components");

            migrationBuilder.DropTable(
                name: "receiver_antenna_components");

            migrationBuilder.DropTable(
                name: "receiver_components");

            migrationBuilder.DropTable(
                name: "video_antenna_components");

            migrationBuilder.DropTable(
                name: "video_transmitter_components");
        }
    }
}
