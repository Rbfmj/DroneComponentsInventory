CREATE TABLE battery_components (
    battery_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    cell_count_s INTEGER,
    nominal_voltage_v REAL,
    capacity_mah INTEGER,
    discharge_rate_c REAL,
    burst_rate_c REAL,
    discharge_connector TEXT,
    balance_connector TEXT,
    chemistry TEXT,
    weight_g INTEGER,
    length_mm INTEGER,
    width_mm INTEGER,
    height_mm INTEGER,
    price REAL
);

CREATE TABLE frame_components (
    frame_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    material TEXT,
    geometry TEXT,
    wheelbase_mm INTEGER,
    max_prop_inch REAL,
    arm_thickness_mm REAL,
    fc_mount_pattern TEXT,
    motor_mount_pattern TEXT,
    max_stack_height_mm INTEGER,
    weight_g INTEGER,
    price REAL
);

CREATE TABLE motors_components (
    motor_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    kv INTEGER,
    stator_size_mm REAL,
    weight_g REAL,
    max_thrust_g INTEGER,
    recommended_prop_inch REAL,
    recommended_voltage_s INTEGER,
    mount_pattern TEXT,
    price REAL
);

CREATE TABLE propellers_components (
    propeller_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    diameter_mm REAL,
    pitch_inch REAL,
    blade_count INTEGER,
    material TEXT,
    weight_g REAL,
    price REAL
);

CREATE TABLE esc_components (
    esc_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    continuous_current_a REAL,
    esc_type TEXT,
    voltage_input_s INTEGER,
    weight_g REAL,
    mount_pattern_mm REAL,
    supported_protocols TEXT,
    price REAL
);

CREATE TABLE fc_components (
    fc_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    mcu_processor TEXT,
    firmware_support TEXT,
    imu_gyro TEXT,
    voltage_input_s TEXT,
    weight_g REAL,
    mount_pattern_mm REAL,
    price REAL
);

CREATE TABLE fpv_camera_components (
    camera_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    resolution_tvl INTEGER,
    sensor_size TEXT,
    lens_focal_mm REAL,
    fov_modes TEXT,
    low_light_lux REAL,
    type_system TEXT,
    weight_g REAL,
    price REAL
);

CREATE TABLE video_transmitter_components (
    vtx_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    max_power_mw REAL,
    voltage_input_s REAL,
    antenna_connector TEXT,
    type TEXT,
    weight_g REAL,
    price REAL
);

CREATE TABLE video_antenna_components (
    antenna_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    operating_frequency_ghz REAL,
    polarization TEXT,
    gain_dbi REAL,
    connector TEXT,
    weight_g REAL,
    price REAL
);

CREATE TABLE receiver_components (
    receiver_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    frequency_band TEXT,
    channels INTEGER,
    protocol TEXT,
    weight_g REAL,
    price REAL
);

CREATE TABLE receiver_antenna_components (
    receiver_antenna_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    frequency_band TEXT,
    gain_dbi REAL,
    connector TEXT,
    weight_g INTEGER,
    price REAL
);

CREATE TABLE radio_controller_components (
    radio_controller_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    frequency_ghz REAL,
    max_channels INTEGER,
    protocols_supported TEXT,
    weight_g INTEGER,
    price REAL
);

CREATE TABLE fpv_goggles_components (
    fpv_goggles_id INTEGER PRIMARY KEY,
    manufacturer TEXT NOT NULL,
    model TEXT NOT NULL,
    display_type TEXT,
    resolution TEXT,
    video_system TEXT,
    weight_g REAL,
    price REAL
);

CREATE TABLE drone_builds (
    build_id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    frame_id INTEGER,
    motor_id INTEGER,
    propeller_id INTEGER,
    esc_id INTEGER,
    battery_id INTEGER,
    fc_id INTEGER,
    camera_id INTEGER,
    vtx_id INTEGER,
    video_antenna_id INTEGER,
    receiver_id INTEGER,
    receiver_antenna_id INTEGER,
    radio_controller_id INTEGER,
    fpv_goggles_id INTEGER,
    created_at TEXT NOT NULL
);

CREATE TABLE assembly_layouts (
    build_id INTEGER PRIMARY KEY,
    layout_json TEXT NOT NULL,
    saved_at TEXT NOT NULL
);