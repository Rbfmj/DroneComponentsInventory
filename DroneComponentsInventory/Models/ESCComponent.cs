using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class ESCComponent
    {
        [Key]
        public int EscId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? EscType { get; set; }
        public double? ContinuousCurrentA { get; set; }
        public int? VoltageInputS { get; set; }
        public double? MountPatternMm { get; set; }
        public string? SupportedProtocols { get; set; }
        public double? WeightG { get; set; }
        public double? Price { get; set; }
    }
}
