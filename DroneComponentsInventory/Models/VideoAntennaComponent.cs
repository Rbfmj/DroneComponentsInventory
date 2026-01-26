using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DroneComponentsInventory.Models
{
    [Table("VideoAntennaComponents")]
    public class VideoAntennaComponent
    {
        [Key]
        public int AntennaId { get; set; }

        [Required]
        public string Manufacturer { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        public string AntennaClass { get; set; } = null!;

        public string Polarization { get; set; } = null!;

        public string Connector { get; set; } = null!;

        // Gain in dBi
        public double GainDbi { get; set; }

        // May be null if not specified
        public double? AxialRatio { get; set; }

        public string RadiationPattern { get; set; } = null!;

        // Keep as string to allow ranges like "5800-5900" or single value "5800"
        public string OperatingFrequencyMhz { get; set; } = null!;

        // Weight in grams
        public int WeightGrams { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
