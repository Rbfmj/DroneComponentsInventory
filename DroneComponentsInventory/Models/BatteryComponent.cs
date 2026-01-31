using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class BatteryComponent
    {
        [Key]
        public int BatteryId { get; set; }
        [Required]
        public string Manufacturer { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        public int? CellCountS { get; set; }
        public double? NominalVoltageV { get; set; }
        public int? CapacityMah { get; set; }
        public double? DischargeRateC { get; set; }
        public double? BurstRateC { get; set; }
        public string? DischargeConnector { get; set; }
        public string? BalanceConnector { get; set; }
        public string? Chemistry { get; set; }
        public int? WeightG { get; set; }
        public int? LengthMm { get; set; }
        public int? WidthMm { get; set; }
        public int? HeightMm { get; set; }
        public double? Price { get; set; }
    }
}
