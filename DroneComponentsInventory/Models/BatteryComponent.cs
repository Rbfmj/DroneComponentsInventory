using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace DroneComponentsInventory.Models
{
    [Table("battery_components")]
    public class BatteryComponent
    {
        [Key]
        [Column("battery_id")]
        public int BatteryId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("model")]
        public string Model { get; set; } = string.Empty;

        [Column("cell_count_s")]
        public int CellCountS { get; set; }

        [Column("nominal_voltage_v", TypeName = "decimal(6,2)")]
        public decimal NominalVoltageV { get; set; }

        [Column("capacity_mah")]
        public int CapacityMah { get; set; }

        [Column("discharge_rate_c")]
        public double DischargeRateC { get; set; }

        [Column("burst_rate_c")]
        public double BurstRateC { get; set; }

        [MaxLength(50)]
        [Column("discharge_connector")]
        public string DischargeConnector { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("balance_connector")]
        public string BalanceConnector { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("chemistry")]
        public string Chemistry { get; set; } = string.Empty;

        [Column("weight_g")]
        public double WeightG { get; set; }

        [Column("length_mm")]
        public double LengthMm { get; set; }

        [Column("width_mm")]
        public double WidthMm { get; set; }

        [Column("height_mm")]
        public double HeightMm { get; set; }

        [Column("price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
    }
}
