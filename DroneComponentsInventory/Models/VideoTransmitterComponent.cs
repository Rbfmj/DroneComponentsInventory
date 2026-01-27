using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class VideoTransmitterComponent
    {
        [Key]
        public int VtxId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? Type { get; set; }
        public int? MaxPowerMw { get; set; }
        public double? VoltageInputS { get; set; }
        public string? MountPatternMm { get; set; }
        public string? ControlProtocols { get; set; }
        public string? AntennaConnector { get; set; }
        public double? WeightG { get; set; }
        public double? Price { get; set; }
    }
}
