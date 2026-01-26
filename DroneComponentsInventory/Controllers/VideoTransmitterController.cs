using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DroneComponentsInventory.Models;

namespace DroneComponentsInventory.Controllers
{
    public class VideoTransmitterController : Controller
    {
        private readonly AppDbContext _context;

        public VideoTransmitterController(AppDbContext context)
        {
            _context = context;
        }

        // GET: VideoTransmitter
        public async Task<IActionResult> Index()
        {
            return View(await _context.VideoTransmitterComponent.ToListAsync());
        }

        // GET: VideoTransmitter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoTransmitterComponent = await _context.VideoTransmitterComponent
                .FirstOrDefaultAsync(m => m.VtxId == id);
            if (videoTransmitterComponent == null)
            {
                return NotFound();
            }

            return View(videoTransmitterComponent);
        }

        // GET: VideoTransmitter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VideoTransmitter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VtxId,Manufacturer,Model,Type,MaxPowerMw,VoltageInputS,MountPatternMm,ControlProtocols,AntennaConnector,WeightG,Price")] VideoTransmitterComponent videoTransmitterComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoTransmitterComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(videoTransmitterComponent);
        }

        // GET: VideoTransmitter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoTransmitterComponent = await _context.VideoTransmitterComponent.FindAsync(id);
            if (videoTransmitterComponent == null)
            {
                return NotFound();
            }
            return View(videoTransmitterComponent);
        }

        // POST: VideoTransmitter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VtxId,Manufacturer,Model,Type,MaxPowerMw,VoltageInputS,MountPatternMm,ControlProtocols,AntennaConnector,WeightG,Price")] VideoTransmitterComponent videoTransmitterComponent)
        {
            if (id != videoTransmitterComponent.VtxId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoTransmitterComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoTransmitterComponentExists(videoTransmitterComponent.VtxId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(videoTransmitterComponent);
        }

        // GET: VideoTransmitter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoTransmitterComponent = await _context.VideoTransmitterComponent
                .FirstOrDefaultAsync(m => m.VtxId == id);
            if (videoTransmitterComponent == null)
            {
                return NotFound();
            }

            return View(videoTransmitterComponent);
        }

        // POST: VideoTransmitter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoTransmitterComponent = await _context.VideoTransmitterComponent.FindAsync(id);
            if (videoTransmitterComponent != null)
            {
                _context.VideoTransmitterComponent.Remove(videoTransmitterComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoTransmitterComponentExists(int id)
        {
            return _context.VideoTransmitterComponent.Any(e => e.VtxId == id);
        }
    }
}
