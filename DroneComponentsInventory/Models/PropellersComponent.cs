using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class PropellersComponent
    {
        [Key]
        public int PropellerId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public double? DiameterMm { get; set; }
        public double? PitchInch { get; set; }
        public int? BladeCount { get; set; }
        public string? Material { get; set; }
        public double? ShaftDiameterMm { get; set; }
        public string? RotationDirection { get; set; }
        public string? RecommendedMotorSize { get; set; }
        public int? RecommendedMotorKv { get; set; }
        public string? FrameClass { get; set; }
        public double? WeightG { get; set; }
        public string? ColorOptions { get; set; }
        public int? IncludedQuantity { get; set; }
        public double? Price { get; set; }  
    }
}
