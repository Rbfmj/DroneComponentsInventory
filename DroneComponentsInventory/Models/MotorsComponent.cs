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
        public int? StatorSizeMm { get; set; }
        public int? Kv { get; set; }
        public string? MountPattern { get; set; }
        public int? WeightG { get; set; }
        public int? RecommendedVoltageS { get; set; }
        public double? RecommendedPropInch { get; set; }
        public int? MaxThrustG { get; set; }
        public double? Price { get; set; }
    }
}
