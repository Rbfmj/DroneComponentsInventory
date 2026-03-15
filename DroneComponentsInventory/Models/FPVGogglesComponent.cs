using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class FPVGogglesComponent
    {
        [Key]
        public int FpvGogglesId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? VideoSystem { get; set; }        
        public string? DisplayType { get; set; }
        public double? ScreenSizeInch { get; set; }
        public string? Resolution { get; set; }
        public int? LatencyMs { get; set; }
        public string? ReceiverType { get; set; }
        public int? DiversityAntennas { get; set; }
        public double? WeightG { get; set; }
        public bool DvrCapability { get; set; } = false;
        public double? BatteryLifeHours { get; set; }
        public string? PowerInput { get; set; }
        public bool IpdAdjustable { get; set; } = false;
        public double? Price { get; set; }
    }
}
