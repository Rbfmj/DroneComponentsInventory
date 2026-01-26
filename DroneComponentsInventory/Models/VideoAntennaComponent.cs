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

        public double GainDbi { get; set; }

        public double? AxialRatio { get; set; }

        public string RadiationPattern { get; set; } = null!;

        public string OperatingFrequencyMhz { get; set; } = null!;

        public int WeightGrams { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
