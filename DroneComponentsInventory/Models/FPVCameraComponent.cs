using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class FPVCameraComponent
    {
        [Key]
        public int CameraId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? TypeSystem { get; set; }
        public string? SensorSize { get; set; }
        public int? ResolutionTvl { get; set; }
        public string? FovModes { get; set; }
        public double? LensFocalMm { get; set; }
        public string? SupportedAspectRatios { get; set; }
        public double? LowLightLux { get; set; }
        public double? WeightG { get; set; }
        public int? MountSizeMm { get; set; }
        public double? Price { get; set; }
    }
}
