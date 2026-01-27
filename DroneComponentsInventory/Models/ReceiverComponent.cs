using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class ReceiverComponent
    {
        [Key]
        public int ReceiverId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? Protocol { get; set; }
        public string? ModulationProtocol { get; set; }
        public string? FrequencyBand { get; set; }
        public int? Channels { get; set; }
        public bool? TelemetrySupport { get; set; }
        public int? AntennaCount { get; set; }
        public string? AntennaType { get; set; }
        public double? RangeKm { get; set; }
        public double? VoltageInputV { get; set; }
        public string? OutputSignal { get; set; }
        public bool? FailsafeSupport { get; set; }
        public string? BindingMethod { get; set; }
        public string? IntendedUse { get; set; }
        public double? WeightG { get; set; }
        public int? LengthMm { get; set; }
        public int? WidthMm { get; set; }
        public int? HeightMm { get; set; }
        public string? MountingType { get; set; }
        public double? Price { get; set; }
    }
}
