using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class VideoAntennaComponent
    {
        [Key]
        public int AntennaId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? AntennaClass { get; set; }
        public string? Polarization { get; set; }
        public string? Connector { get; set; }
        public double? GainDbi { get; set; }
        public double? AxialRatio { get; set; }
        public string? RadiationPattern { get; set; }
        public double? OperatingFrequencyGhz { get; set; }
        public int? WeightG { get; set; }
        public double? Price { get; set; }
    }
}
