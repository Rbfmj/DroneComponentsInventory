using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DroneComponentsInventory.Models
{
    public class DroneBuild
    {
        [Key]
        public int BuildId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public int? FrameId { get; set; }
        [ForeignKey("FrameId")]
        public FrameComponent? Frame { get; set; }

        public int? MotorId { get; set; }
        [ForeignKey("MotorId")]
        public MotorsComponent? Motor { get; set; }

        public int? PropellerId { get; set; }
        [ForeignKey("PropellerId")]
        public PropellersComponent? Propeller { get; set; }

        public int? EscId { get; set; }
        [ForeignKey("EscId")]
        public ESCComponent? Esc { get; set; }

        public int? BatteryId { get; set; }
        [ForeignKey("BatteryId")]
        public BatteryComponent? Battery { get; set; }

        public int? FcId { get; set; }
        [ForeignKey("FcId")]
        public FCComponent? Fc { get; set; }

        public int? CameraId { get; set; }
        [ForeignKey("CameraId")]
        public FPVCameraComponent? Camera { get; set; }

        public int? VtxId { get; set; }
        [ForeignKey("VtxId")]
        public VideoTransmitterComponent? Vtx { get; set; }

        public int? VideoAntennaId { get; set; }
        [ForeignKey("VideoAntennaId")]
        public VideoAntennaComponent? VideoAntenna { get; set; }

        public int? ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public ReceiverComponent? Receiver { get; set; }

        public int? ReceiverAntennaId { get; set; }
        [ForeignKey("ReceiverAntennaId")]
        public ReceiverAntennaComponent? ReceiverAntenna { get; set; }

        public int? RadioControllerId { get; set; }
        [ForeignKey("RadioControllerId")]
        public RadioControllerComponent? RadioController { get; set; }

        public int? FpvGogglesId { get; set; }
        [ForeignKey("FpvGogglesId")]
        public FPVGogglesComponent? FpvGoggles { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
