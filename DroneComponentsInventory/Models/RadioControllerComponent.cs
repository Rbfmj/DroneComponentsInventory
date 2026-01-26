using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

    namespace DroneComponentsInventory.Models
{
    [Table("radio_controller_components")]
    public class RadioControllerComponent
    {
        [Key]
        [Column("radio_controller_id")]
        public int RadioControllerId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("model")]
        public string Model { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("controller_style")]
        public string? ControllerStyle { get; set; }

        [Column("frequency_ghz", TypeName = "decimal(6,3)")]
        public decimal? FrequencyGhz { get; set; }

        [MaxLength(200)]
        [Column("protocols_supported")]
        public string? ProtocolsSupported { get; set; }

        [Column("max_channels")]
        public int? MaxChannels { get; set; }

        [Column("output_power_mw", TypeName = "decimal(8,2)")]
        public decimal? OutputPowerMw { get; set; }

        [Column("telemetry_support")]
        public bool? TelemetrySupport { get; set; }

        [MaxLength(50)]
        [Column("screen_type")]
        public string? ScreenType { get; set; }

        [MaxLength(50)]
        [Column("gimbal_type")]
        public string? GimbalType { get; set; }

        [MaxLength(50)]
        [Column("battery_type")]
        public string? BatteryType { get; set; }

        [Column("weight_g", TypeName = "decimal(8,2)")]
        public decimal? WeightG { get; set; }

        [Column("length_mm")]
        public int? LengthMm { get; set; }

        [Column("width_mm")]
        public int? WidthMm { get; set; }

        [Column("height_mm")]
        public int? HeightMm { get; set; }

        [MaxLength(200)]
        [Column("firmware_support")]
        public string? FirmwareSupport { get; set; }

        [Column("price")]
        [Precision(18, 2)]
        public decimal? Price { get; set; }
    }
}
