using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DroneComponentsInventory.Models
{
    [Table("fpv_goggles_components")]
    public class FPVGogglesComponent
    {
        [Key]
        [Column("fpv_goggles_id")]
        public int FPVGogglesId { get; set; }

        [Required]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = null!;

        [Required]
        [Column("model")]
        public string Model { get; set; } = null!;

        [Column("video_system")]
        public string? VideoSystem { get; set; }        

        [Column("display_type")]
        public string? DisplayType { get; set; }

        [Column("screen_size_inch")]
        public double? ScreenSizeInch { get; set; }

        [Column("resolution")]
        public string? Resolution { get; set; }

        [Column("latency_ms")]
        public int? LatencyMs { get; set; }

        [Column("receiver_type")]
        public string? ReceiverType { get; set; }

        [Column("diversity_antennas")]
        public int? DiversityAntennas { get; set; }

        [Column("weight_g")]
        public int? WeightGrams { get; set; }

        [Column("dvr_capability")]
        public bool? DvrCapability { get; set; }

        [Column("battery_life_hours")]
        public double? BatteryLifeHours { get; set; }

        [Column("power_input")]
        public string? PowerInput { get; set; }

        [Column("ipd_adjustable")]
        public bool? IPDAdjustable { get; set; }

        [Column("price", TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
    }
}
