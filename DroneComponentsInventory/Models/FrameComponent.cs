using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class FrameComponent
    {
        [Key]
        public int FrameId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public int? WheelbaseMm { get; set; }
        public double? MaxPropInch { get; set; }
        public string? Geometry { get; set; }
        public string? Material { get; set; }
        public int? FrameWeightG { get; set; }
        public int? ArmThicknessMm { get; set; }
        public string? MotorMountPattern { get; set; }
        public string? FcMountPattern { get; set; }
        public int? MaxStackHeightMm { get; set; }
        public double? Price { get; set; }
    }
}
