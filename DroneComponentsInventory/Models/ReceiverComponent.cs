using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DroneComponentsInventory.Models
{
    public class ReceiverComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiverId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Protocol { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ModulationProtocol { get; set; } = string.Empty;

        [MaxLength(100)]
        public string FrequencyBand { get; set; } = string.Empty;

        public int? Channels { get; set; }

        public bool TelemetrySupport { get; set; }

        public int? AntennaCount { get; set; }

        [MaxLength(100)]
        public string AntennaType { get; set; } = string.Empty;

        public double? RangeKm { get; set; }

        public double? VoltageInputV { get; set; }

        [MaxLength(100)]
        public string OutputSignal { get; set; } = string.Empty;

        public bool FailsafeSupport { get; set; }

        [MaxLength(100)]
        public string BindingMethod { get; set; } = string.Empty;

        [MaxLength(200)]
        public string IntendedUse { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? WeightG { get; set; }

        public int? LengthMm { get; set; }

        public int? WidthMm { get; set; }

        public int? HeightMm { get; set; }

        [MaxLength(100)]
        public string MountingType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }
    }
}
