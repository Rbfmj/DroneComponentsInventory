using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DroneComponentsInventory.Models
{
    [Table("fpv_camera_components")]
    public class FPVCameraComponent
    {
        [Key]
        [Column("camera_id")]
        public int CameraId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("model")]
        public string Model { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("type_system")]
        public string? TypeSystem { get; set; }

        [MaxLength(50)]
        [Column("sensor_size")]
        public string? SensorSize { get; set; }

        [Column("resolution_tvl")]
        public int? ResolutionTvl { get; set; }

        [Column("fov_modes")]
        public string? FovModes { get; set; }

        [Column("lens_focal_mm")]
        public decimal? LensFocalMm { get; set; }

        [Column("supported_aspect_ratios")]
        public string? SupportedAspectRatios { get; set; }

        [Column("low_light_lux")]
        public decimal? LowLightLux { get; set; }

        [Column("weight_g", TypeName = "decimal(8,2)")]
        public decimal? WeightG { get; set; }

        [Column("mount_size_mm")]
        public int? MountSizeMm { get; set; }

        [Column("price")]
        [Precision(18, 2)]
        public decimal? Price { get; set; }
    }
}
