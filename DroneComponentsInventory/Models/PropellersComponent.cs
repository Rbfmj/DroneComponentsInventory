using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DroneComponentsInventory.Models
{
    [Table("propellers_components")]
    public class PropellersComponent
    {
        [Key]
        [Column("propeller_id")]
        public int PropellerId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = null!;

        [MaxLength(100)]
        [Column("model")]
        public string Model { get; set; } = null!;

        // diameter in millimeters
        [Column("diameter_mm")]
        public int DiameterMm { get; set; }

        // pitch in inches (use decimal for fractional values)
        [Column("pitch_inch")]
        public decimal PitchInch { get; set; }

        [Column("blade_count")]
        public int BladeCount { get; set; }

        [MaxLength(50)]
        [Column("material")]
        public string Material { get; set; } = null!;

        // shaft diameter in millimeters (may be fractional)
        [Column("shaft_diameter_mm")]
        public decimal ShaftDiameterMm { get; set; }

        [MaxLength(20)]
        [Column("rotation_direction")]
        public string RotationDirection { get; set; } = null!; // e.g., "CW" or "CCW"

        [MaxLength(100)]
        [Column("recommended_motor_size")]
        public string RecommendedMotorSize { get; set; } = null!;

        [Column("recommended_motor_kv")]
        public int RecommendedMotorKv { get; set; }

        [MaxLength(50)]
        [Column("frame_class")]
        public string FrameClass { get; set; } = null!;

        // weight in grams
        [Column("weight_g")]
        public decimal WeightG { get; set; }

        [MaxLength(200)]
        [Column("color_options")]
        public string ColorOptions { get; set; } = null!; // comma-separated or JSON

        [Column("included_quantity")]
        public int IncludedQuantity { get; set; }

        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }  
    }
}
