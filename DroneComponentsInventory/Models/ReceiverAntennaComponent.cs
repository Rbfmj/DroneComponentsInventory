using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class ReceiverAntennaComponent
    {
        [Key]
        public int ReceiverAntennaId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? FrequencyBand { get; set; }
        public string? Polarization { get; set; }
        public string? Connector { get; set; }
        public string? ConnectorGender { get; set; }
        public double? GainDbi { get; set; }
        public string? RadiationPattern { get; set; }
        public string? CompatibleReceivers { get; set; }
        public string? MountType { get; set; }
        public int? CableLengthMm { get; set; }
        public double? WeightG { get; set; }
        public int? LengthMm { get; set; }
        public double? Price { get; set; }
    }
}
