namespace DroneComponentsInventory.Models
{
    public class DroneBuilderViewModel
    {
        public List<FrameComponent> Frames { get; set; } = [];
        public List<MotorsComponent> Motors { get; set; } = [];
        public List<PropellersComponent> Propellers { get; set; } = [];
        public List<ESCComponent> Escs { get; set; } = [];
        public List<BatteryComponent> Batteries { get; set; } = [];
        public List<FCComponent> FlightControllers { get; set; } = [];
        public List<FPVCameraComponent> Cameras { get; set; } = [];
        public List<VideoTransmitterComponent> Vtxs { get; set; } = [];
        public List<VideoAntennaComponent> VideoAntennas { get; set; } = [];
        public List<ReceiverComponent> Receivers { get; set; } = [];
        public List<ReceiverAntennaComponent> ReceiverAntennas { get; set; } = [];
        public List<RadioControllerComponent> RadioControllers { get; set; } = [];
        public List<FPVGogglesComponent> FpvGoggles { get; set; } = [];
    }
}
