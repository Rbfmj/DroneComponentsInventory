using System.ComponentModel.DataAnnotations;

    namespace DroneComponentsInventory.Models
{
    public class RadioControllerComponent
    {
        [Key]
        public int RadioControllerId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public string? ControllerStyle { get; set; }
        public double? FrequencyGhz { get; set; }
        public string? ProtocolsSupported { get; set; }
        public int? MaxChannels { get; set; }
        public double? OutputPowerMw { get; set; }
        public bool TelemetrySupport { get; set; } = false;
        public string? ScreenType { get; set; }
        public string? GimbalType { get; set; }
        public string? BatteryType { get; set; }
        public int? WeightG { get; set; }
        public string? FirmwareSupport { get; set; }
        public double? Price { get; set; }
    }
}
