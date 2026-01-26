using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace DroneComponentsInventory.Models
{
    [Table("fc_components")]
    public class FCComponent
    {
        [Key]
        [Column("fc_id")]
        public int FcId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column("model")]
        public string Model { get; set; } = null!;

        [MaxLength(100)]
        [Column("mcu_processor")]
        public string? McuProcessor { get; set; }

        [MaxLength(100)]
        [Column("imu_gyro")]
        public string? ImuGyro { get; set; }

        [MaxLength(50)]
        [Column("mount_pattern_mm")]
        public string? MountPatternMm { get; set; }

        [MaxLength(20)]
        [Column("voltage_input_s")]
        public string? VoltageInputS { get; set; }

        [MaxLength(100)]
        [Column("firmware_support")]
        public string? FirmwareSupport { get; set; }

        [Column("weight_g", TypeName = "decimal(8,2)")]
        public decimal? WeightG { get; set; }
    }
}
