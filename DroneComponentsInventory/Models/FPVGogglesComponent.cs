using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class FPVGogglesComponent
    {
        [Key]
        public int FPVGogglesId { get; set; }
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
        public int? WeightGrams { get; set; }
        public bool? DvrCapability { get; set; }
        public double? BatteryLifeHours { get; set; }
        public string? PowerInput { get; set; }
        public bool? IPDAdjustable { get; set; }
        public double? Price { get; set; }
    }
}
