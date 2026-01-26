using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DroneComponentsInventory.Models
{
    [Table("VideoTransmitterComponents")]
    public class VideoTransmitterComponent
    {
        [Key]
        [Column("vtx_id")]
        public int VtxId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column("model")]
        public string Model { get; set; } = null!;

        [MaxLength(50)]
        [Column("type")]
        public string? Type { get; set; }

        [Column("max_power_mw")]
        public int? MaxPowerMw { get; set; }

        [Column("voltage_input_s")]
        public decimal? VoltageInputS { get; set; }

        [MaxLength(50)]
        [Column("mount_pattern_mm")]
        public string? MountPatternMm { get; set; }

        [Column("control_protocols")]
        public string? ControlProtocols { get; set; }

        [MaxLength(50)]
        [Column("antenna_connector")]
        public string? AntennaConnector { get; set; }

        [Column("weight_g")]
        public decimal? WeightG { get; set; }

        [Column("price")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
    }
}
