using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class MotorsComponent
    {
        [Key]
        public int MotorId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public double? StatorSizeMm { get; set; }
        public int? Kv { get; set; }
        public string? MountPattern { get; set; }
        public double? WeightGrams { get; set; }
        public int? RecommendedVoltageS { get; set; }
        public string? RecommendedPropInch { get; set; }
        public double? MaxThrustGrams { get; set; }
        public double? Price { get; set; }
    }
}
