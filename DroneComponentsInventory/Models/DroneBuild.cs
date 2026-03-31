using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class DroneBuild
    {
        [Key]
        public int BuildId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int? FrameId { get; set; }
        public FrameComponent? Frame { get; set; }

        public int? MotorId { get; set; }
        public MotorsComponent? Motor { get; set; }

        public int? PropellerId { get; set; }
        public PropellersComponent? Propeller { get; set; }

        public int? EscId { get; set; }
        public ESCComponent? Esc { get; set; }

        public int? BatteryId { get; set; }
        public BatteryComponent? Battery { get; set; }

        public int? FcId { get; set; }
        public FCComponent? Fc { get; set; }

        public int? CameraId { get; set; }
        public FPVCameraComponent? Camera { get; set; }

        public int? VtxId { get; set; }
        public VideoTransmitterComponent? Vtx { get; set; }

        public int? VideoAntennaId { get; set; }
        public VideoAntennaComponent? VideoAntenna { get; set; }

        public int? ReceiverId { get; set; }
        public ReceiverComponent? Receiver { get; set; }

        public int? ReceiverAntennaId { get; set; }
        public ReceiverAntennaComponent? ReceiverAntenna { get; set; }

        public int? RadioControllerId { get; set; }
        public RadioControllerComponent? RadioController { get; set; }

        public int? FpvGogglesId { get; set; }
        public FPVGogglesComponent? FpvGoggles { get; set; }
    }
}
