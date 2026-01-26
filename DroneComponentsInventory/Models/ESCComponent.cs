using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

    namespace DroneComponentsInventory.Models
{
    [Table("esc_components")]
    public class ESCComponent
    {
        [Key]
        [Column("esc_id")]
        public int EscId { get; set; }

        [Required]
        [Column("manufacturer")]
        [StringLength(100)]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [Column("model")]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        // 'type' is a common column name; map it but use a clearer C# property name
        [Column("type")]
        [StringLength(50)]
        public string? EscType { get; set; }

        [Column("continuous_current_a")]
        public decimal? ContinuousCurrentA { get; set; }

        [Column("voltage_input_s")]
        public int? VoltageInputS { get; set; }

        [Column("mount_pattern_mm")]
        public int? MountPatternMm { get; set; }

        [Column("supported_protocols")]
        public string? SupportedProtocols { get; set; }

        [Column("weight_g")]
        public int? WeightG { get; set; }

        [Column("price")]
        [Precision(18, 2)]
        public decimal? Price { get; set; }
    }
}
