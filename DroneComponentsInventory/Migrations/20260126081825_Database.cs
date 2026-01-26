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
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    cell_count_s = table.Column<int>(type: "INTEGER", nullable: false),
                    nominal_voltage_v = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    capacity_mah = table.Column<int>(type: "INTEGER", nullable: false),
                    discharge_rate_c = table.Column<double>(type: "REAL", nullable: false),
                    burst_rate_c = table.Column<double>(type: "REAL", nullable: false),
                    discharge_connector = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    balance_connector = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    chemistry = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    weight_g = table.Column<double>(type: "REAL", nullable: false),
                    length_mm = table.Column<double>(type: "REAL", nullable: false),
                    width_mm = table.Column<double>(type: "REAL", nullable: false),
                    height_mm = table.Column<double>(type: "REAL", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battery_components", x => x.battery_id);
                });

            migrationBuilder.CreateTable(
                name: "esc_components",
                columns: table => new
                {
                    esc_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    continuous_current_a = table.Column<decimal>(type: "TEXT", nullable: true),
                    voltage_input_s = table.Column<int>(type: "INTEGER", nullable: true),
                    mount_pattern_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    supported_protocols = table.Column<string>(type: "TEXT", nullable: true),
                    weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_esc_components", x => x.esc_id);
                });

            migrationBuilder.CreateTable(
                name: "fc_components",
                columns: table => new
                {
                    fc_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    mcu_processor = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    imu_gyro = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    mount_pattern_mm = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    voltage_input_s = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    firmware_support = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    weight_g = table.Column<decimal>(type: "decimal(8,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fc_components", x => x.fc_id);
                });

            migrationBuilder.CreateTable(
                name: "fpv_camera_components",
                columns: table => new
                {
                    camera_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    type_system = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    sensor_size = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    resolution_tvl = table.Column<int>(type: "INTEGER", nullable: true),
                    fov_modes = table.Column<string>(type: "TEXT", nullable: true),
                    lens_focal_mm = table.Column<decimal>(type: "TEXT", nullable: true),
                    supported_aspect_ratios = table.Column<string>(type: "TEXT", nullable: true),
                    low_light_lux = table.Column<decimal>(type: "TEXT", nullable: true),
                    weight_g = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    mount_size_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fpv_camera_components", x => x.camera_id);
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
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fpv_goggles_components", x => x.fpv_goggles_id);
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
                    frame_weight_g = table.Column<int>(type: "INTEGER", nullable: true),
                    arm_thickness_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    motor_mount_pattern = table.Column<string>(type: "TEXT", nullable: true),
                    fc_mount_pattern = table.Column<string>(type: "TEXT", nullable: true),
                    max_stack_height_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_frame_components", x => x.frame_id);
                });

            migrationBuilder.CreateTable(
                name: "MotorsComponents",
                columns: table => new
                {
                    MotorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    StatorSizeMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    Kv = table.Column<int>(type: "INTEGER", nullable: true),
                    MountPattern = table.Column<string>(type: "TEXT", nullable: true),
                    WeightGrams = table.Column<decimal>(type: "TEXT", nullable: true),
                    RecommendedVoltageS = table.Column<int>(type: "INTEGER", nullable: true),
                    RecommendedPropInch = table.Column<string>(type: "TEXT", nullable: true),
                    MaxThrustGrams = table.Column<decimal>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorsComponents", x => x.MotorId);
                });

            migrationBuilder.CreateTable(
                name: "propellers_components",
                columns: table => new
                {
                    propeller_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    diameter_mm = table.Column<int>(type: "INTEGER", nullable: false),
                    pitch_inch = table.Column<decimal>(type: "TEXT", nullable: false),
                    blade_count = table.Column<int>(type: "INTEGER", nullable: false),
                    material = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    shaft_diameter_mm = table.Column<decimal>(type: "TEXT", nullable: false),
                    rotation_direction = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    recommended_motor_size = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    recommended_motor_kv = table.Column<int>(type: "INTEGER", nullable: false),
                    frame_class = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    weight_g = table.Column<decimal>(type: "TEXT", nullable: false),
                    color_options = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    included_quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_propellers_components", x => x.propeller_id);
                });

            migrationBuilder.CreateTable(
                name: "radio_controller_components",
                columns: table => new
                {
                    radio_controller_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    controller_style = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    frequency_ghz = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    protocols_supported = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    max_channels = table.Column<int>(type: "INTEGER", nullable: true),
                    output_power_mw = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    telemetry_support = table.Column<bool>(type: "INTEGER", nullable: true),
                    screen_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    gimbal_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    battery_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    weight_g = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    length_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    width_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    height_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    firmware_support = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radio_controller_components", x => x.radio_controller_id);
                });

            migrationBuilder.CreateTable(
                name: "receiver_antenna_components",
                columns: table => new
                {
                    receiver_antenna_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    frequency_band = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    polarization = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    connector = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    connector_gender = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    gain_dbi = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    radiation_pattern = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    compatible_receivers = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    mount_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    cable_length_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    weight_g = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    length_mm = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receiver_antenna_components", x => x.receiver_antenna_id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiverComponents",
                columns: table => new
                {
                    ReceiverId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Protocol = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ModulationProtocol = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FrequencyBand = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Channels = table.Column<int>(type: "INTEGER", nullable: true),
                    TelemetrySupport = table.Column<bool>(type: "INTEGER", nullable: false),
                    AntennaCount = table.Column<int>(type: "INTEGER", nullable: true),
                    AntennaType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RangeKm = table.Column<double>(type: "REAL", nullable: true),
                    VoltageInputV = table.Column<double>(type: "REAL", nullable: true),
                    OutputSignal = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FailsafeSupport = table.Column<bool>(type: "INTEGER", nullable: false),
                    BindingMethod = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IntendedUse = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    WeightG = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    LengthMm = table.Column<int>(type: "INTEGER", nullable: true),
                    WidthMm = table.Column<int>(type: "INTEGER", nullable: true),
                    HeightMm = table.Column<int>(type: "INTEGER", nullable: true),
                    MountingType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiverComponents", x => x.ReceiverId);
                });

            migrationBuilder.CreateTable(
                name: "VideoAntennaComponents",
                columns: table => new
                {
                    AntennaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    AntennaClass = table.Column<string>(type: "TEXT", nullable: false),
                    Polarization = table.Column<string>(type: "TEXT", nullable: false),
                    Connector = table.Column<string>(type: "TEXT", nullable: false),
                    GainDbi = table.Column<double>(type: "REAL", nullable: false),
                    AxialRatio = table.Column<double>(type: "REAL", nullable: true),
                    RadiationPattern = table.Column<string>(type: "TEXT", nullable: false),
                    OperatingFrequencyMhz = table.Column<string>(type: "TEXT", nullable: false),
                    WeightGrams = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoAntennaComponents", x => x.AntennaId);
                });

            migrationBuilder.CreateTable(
                name: "VideoTransmitterComponents",
                columns: table => new
                {
                    vtx_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    max_power_mw = table.Column<int>(type: "INTEGER", nullable: true),
                    voltage_input_s = table.Column<decimal>(type: "TEXT", nullable: true),
                    mount_pattern_mm = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    control_protocols = table.Column<string>(type: "TEXT", nullable: true),
                    antenna_connector = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    weight_g = table.Column<decimal>(type: "TEXT", nullable: true),
                    price = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoTransmitterComponents", x => x.vtx_id);
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
                name: "MotorsComponents");

            migrationBuilder.DropTable(
                name: "propellers_components");

            migrationBuilder.DropTable(
                name: "radio_controller_components");

            migrationBuilder.DropTable(
                name: "receiver_antenna_components");

            migrationBuilder.DropTable(
                name: "ReceiverComponents");

            migrationBuilder.DropTable(
                name: "VideoAntennaComponents");

            migrationBuilder.DropTable(
                name: "VideoTransmitterComponents");
        }
    }
}
