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
    public class VideoAntennaController : Controller
    {
        private readonly AppDbContext _context;

        public VideoAntennaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: VideoAntenna
        public async Task<IActionResult> Index()
        {
            return View(await _context.VideoAntennaComponents.ToListAsync());
        }

        // GET: VideoAntenna/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoAntennaComponent = await _context.VideoAntennaComponents
                .FirstOrDefaultAsync(m => m.AntennaId == id);
            if (videoAntennaComponent == null)
            {
                return NotFound();
            }

            return View(videoAntennaComponent);
        }

        // GET: VideoAntenna/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VideoAntenna/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AntennaId,Manufacturer,Model,AntennaClass,Polarization,Connector,GainDbi,AxialRatio,RadiationPattern,OperatingFrequencyMhz,WeightGrams,Price")] VideoAntennaComponent videoAntennaComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoAntennaComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(videoAntennaComponent);
        }

        // GET: VideoAntenna/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoAntennaComponent = await _context.VideoAntennaComponents.FindAsync(id);
            if (videoAntennaComponent == null)
            {
                return NotFound();
            }
            return View(videoAntennaComponent);
        }

        // POST: VideoAntenna/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AntennaId,Manufacturer,Model,AntennaClass,Polarization,Connector,GainDbi,AxialRatio,RadiationPattern,OperatingFrequencyMhz,WeightGrams,Price")] VideoAntennaComponent videoAntennaComponent)
        {
            if (id != videoAntennaComponent.AntennaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoAntennaComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoAntennaComponentExists(videoAntennaComponent.AntennaId))
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
            return View(videoAntennaComponent);
        }

        // GET: VideoAntenna/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoAntennaComponent = await _context.VideoAntennaComponents
                .FirstOrDefaultAsync(m => m.AntennaId == id);
            if (videoAntennaComponent == null)
            {
                return NotFound();
            }

            return View(videoAntennaComponent);
        }

        // POST: VideoAntenna/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoAntennaComponent = await _context.VideoAntennaComponents.FindAsync(id);
            if (videoAntennaComponent != null)
            {
                _context.VideoAntennaComponents.Remove(videoAntennaComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoAntennaComponentExists(int id)
        {
            return _context.VideoAntennaComponents.Any(e => e.AntennaId == id);
        }
    }
}
