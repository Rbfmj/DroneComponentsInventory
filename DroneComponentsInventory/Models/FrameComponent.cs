using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DroneComponentsInventory.Models
{
    [Table("frame_components")]
    public class FrameComponent
    {
        [Key]
        [Column("frame_id")]
        public int FrameId { get; set; }

        [Required]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [Column("model")]
        public string Model { get; set; } = string.Empty;

        [Column("wheelbase_mm")]
        public int? WheelbaseMm { get; set; }

        [Column("max_prop_inch")]
        public double? MaxPropInch { get; set; }

        [Column("geometry")]
        public string? Geometry { get; set; }

        [Column("material")]
        public string? Material { get; set; }

        [Column("frame_weight_g")]
        public int? FrameWeightG { get; set; }

        [Column("arm_thickness_mm")]
        public int? ArmThicknessMm { get; set; }

        [Column("motor_mount_pattern")]
        public string? MotorMountPattern { get; set; }

        [Column("fc_mount_pattern")]
        public string? FcMountPattern { get; set; }

        [Column("max_stack_height_mm")]
        public int? MaxStackHeightMm { get; set; }

        [Column("price")]
        [Precision(18, 2)]
        public decimal? Price { get; set; }
    }
}
