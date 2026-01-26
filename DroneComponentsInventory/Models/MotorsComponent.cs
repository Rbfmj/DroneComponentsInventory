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
        public decimal? StatorSizeMm { get; set; }

        public int? Kv { get; set; }

        public string? MountPattern { get; set; }

        public decimal? WeightGrams { get; set; }

        public int? RecommendedVoltageS { get; set; }

        public string? RecommendedPropInch { get; set; }

        public decimal? MaxThrustGrams { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }
    }
}
