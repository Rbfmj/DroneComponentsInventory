using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DroneComponentsInventory.Models
{
    [Table("receiver_antenna_components")]
    public class ReceiverAntennaComponent
    {
        [Key]
        [Column("receiver_antenna_id")]
        public int ReceiverAntennaId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("model")]
        public string Model { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("frequency_band")]
        public string? FrequencyBand { get; set; }

        [MaxLength(50)]
        [Column("polarization")]
        public string? Polarization { get; set; }

        [MaxLength(50)]
        [Column("connector")]
        public string? Connector { get; set; }

        [MaxLength(20)]
        [Column("connector_gender")]
        public string? ConnectorGender { get; set; }

        [Column("gain_dbi", TypeName = "decimal(6,2)")]
        public decimal? GainDbi { get; set; }

        [MaxLength(50)]
        [Column("radiation_pattern")]
        public string? RadiationPattern { get; set; }

        [MaxLength(200)]
        [Column("compatible_receivers")]
        public string? CompatibleReceivers { get; set; }

        [MaxLength(50)]
        [Column("mount_type")]
        public string? MountType { get; set; }

        [Column("cable_length_mm")]
        public int? CableLengthMm { get; set; }

        [Column("weight_g", TypeName = "decimal(8,2)")]
        public decimal? WeightG { get; set; }

        [Column("length_mm")]
        public int? LengthMm { get; set; }

        [Column("price")]
        [Precision(18, 2)]
        public decimal? Price { get; set; }
    }
}
