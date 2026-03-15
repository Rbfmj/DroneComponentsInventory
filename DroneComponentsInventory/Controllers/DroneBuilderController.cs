using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DroneComponentsInventory.Models;
using DroneComponentsInventory.Services;

namespace DroneComponentsInventory.Controllers
{
    public class DroneBuilderController : Controller
    {
        private readonly AppDbContext _context;

        public DroneBuilderController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UseFluidLayout = true;

            var vm = new DroneBuilderViewModel
            {
                Frames = await _context.FrameComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                Motors = await _context.MotorsComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                Propellers = await _context.PropellersComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                Escs = await _context.ESCComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                Batteries = await _context.BatteryComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                FlightControllers = await _context.FCComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                Cameras = await _context.FPVCameraComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                Vtxs = await _context.VideoTransmitterComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                VideoAntennas = await _context.VideoAntennaComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                Receivers = await _context.ReceiverComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                ReceiverAntennas = await _context.ReceiverAntennaComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                RadioControllers = await _context.RadioControllerComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync(),
                FpvGoggles = await _context.FPVGogglesComponents.OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CheckCompatibility([FromBody] BuildSelectionDto selection)
        {
            var components = new BuildComponents
            {
                Frame = selection.FrameId.HasValue
                    ? await _context.FrameComponents.FindAsync(selection.FrameId.Value) : null,
                Motor = selection.MotorId.HasValue
                    ? await _context.MotorsComponents.FindAsync(selection.MotorId.Value) : null,
                Propeller = selection.PropellerId.HasValue
                    ? await _context.PropellersComponents.FindAsync(selection.PropellerId.Value) : null,
                Esc = selection.EscId.HasValue
                    ? await _context.ESCComponents.FindAsync(selection.EscId.Value) : null,
                Battery = selection.BatteryId.HasValue
                    ? await _context.BatteryComponents.FindAsync(selection.BatteryId.Value) : null,
                Fc = selection.FcId.HasValue
                    ? await _context.FCComponents.FindAsync(selection.FcId.Value) : null,
                Camera = selection.CameraId.HasValue
                    ? await _context.FPVCameraComponents.FindAsync(selection.CameraId.Value) : null,
                Vtx = selection.VtxId.HasValue
                    ? await _context.VideoTransmitterComponents.FindAsync(selection.VtxId.Value) : null,
                VideoAntenna = selection.VideoAntennaId.HasValue
                    ? await _context.VideoAntennaComponents.FindAsync(selection.VideoAntennaId.Value) : null,
                Receiver = selection.ReceiverId.HasValue
                    ? await _context.ReceiverComponents.FindAsync(selection.ReceiverId.Value) : null,
                ReceiverAntenna = selection.ReceiverAntennaId.HasValue
                    ? await _context.ReceiverAntennaComponents.FindAsync(selection.ReceiverAntennaId.Value) : null,
                RadioController = selection.RadioControllerId.HasValue
                    ? await _context.RadioControllerComponents.FindAsync(selection.RadioControllerId.Value) : null,
                FpvGoggles = selection.FpvGogglesId.HasValue
                    ? await _context.FPVGogglesComponents.FindAsync(selection.FpvGogglesId.Value) : null
            };

            var service = new CompatibilityCheckService();
            var results = service.RunAllChecks(components);

            return Json(results);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBuild([FromBody] BuildSelectionDto selection)
        {
            if (string.IsNullOrWhiteSpace(selection.Name))
                return Json(new { success = false, error = "Name is required" });

            var build = new DroneBuild
            {
                Name = selection.Name,
                FrameId = selection.FrameId,
                MotorId = selection.MotorId,
                PropellerId = selection.PropellerId,
                EscId = selection.EscId,
                BatteryId = selection.BatteryId,
                FcId = selection.FcId,
                CameraId = selection.CameraId,
                VtxId = selection.VtxId,
                VideoAntennaId = selection.VideoAntennaId,
                ReceiverId = selection.ReceiverId,
                ReceiverAntennaId = selection.ReceiverAntennaId,
                RadioControllerId = selection.RadioControllerId,
                FpvGogglesId = selection.FpvGogglesId,
                CreatedAt = DateTime.UtcNow
            };

            _context.DroneBuilds.Add(build);
            await _context.SaveChangesAsync();

            return Json(new { success = true, buildId = build.BuildId });
        }
    }
}
