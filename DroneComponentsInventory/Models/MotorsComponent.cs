using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DroneComponentsInventory.Models
{
    public class MotorsComponent
    {
        [Key]
        public int MotorId { get; set; }

        [Required, StringLength(100)]
        public string Manufacturer { get; set; } = null!;

        [Required, StringLength(100)]
        public string Model { get; set; } = null!;

        // Stator size in millimeters (e.g. 22.0)
        public decimal? StatorSizeMm { get; set; }

        // Kv (RPM per volt)
        public int? Kv { get; set; }

        // Mount pattern description (e.g. "M3 x 16mm")
        public string? MountPattern { get; set; }

        // Weight in grams
        public decimal? WeightGrams { get; set; }

        // Recommended battery cell count (e.g. 3 for 3S)
        public int? RecommendedVoltageS { get; set; }

        // Recommended propeller size in inches (e.g. "5x3")
        public string? RecommendedPropInch { get; set; }

        // Maximum thrust in grams
        public decimal? MaxThrustGrams { get; set; }

        // Price in project currency
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }
    }
}
