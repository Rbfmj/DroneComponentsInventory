using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class FCComponent
    {
        [Key]
        public int FcId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? McuProcessor { get; set; }
        public string? ImuGyro { get; set; }
        public double? MountPatternMm { get; set; }
        public string? VoltageInputS { get; set; }
        public string? FirmwareSupport { get; set; }
        public double? WeightG { get; set; }
        public double? Price { get; set; }
    }
}