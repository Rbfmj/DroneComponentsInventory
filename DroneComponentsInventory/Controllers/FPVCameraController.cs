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
    public class FPVCameraController : Controller
    {
        private readonly AppDbContext _context;

        public FPVCameraController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FPVCamera
        public async Task<IActionResult> Index()
        {
            return View(await _context.FPVCameraComponents.ToListAsync());
        }

        // GET: FPVCamera/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fPVCameraComponent = await _context.FPVCameraComponents
                .FirstOrDefaultAsync(m => m.CameraId == id);
            if (fPVCameraComponent == null)
            {
                return NotFound();
            }

            return View(fPVCameraComponent);
        }

        // GET: FPVCamera/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FPVCamera/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CameraId,Manufacturer,Model,TypeSystem,SensorSize,ResolutionTvl,FovModes,LensFocalMm,SupportedAspectRatios,LowLightLux,WeightG,MountSizeMm,Price")] FPVCameraComponent fPVCameraComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fPVCameraComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fPVCameraComponent);
        }

        // GET: FPVCamera/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fPVCameraComponent = await _context.FPVCameraComponents.FindAsync(id);
            if (fPVCameraComponent == null)
            {
                return NotFound();
            }
            return View(fPVCameraComponent);
        }

        // POST: FPVCamera/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CameraId,Manufacturer,Model,TypeSystem,SensorSize,ResolutionTvl,FovModes,LensFocalMm,SupportedAspectRatios,LowLightLux,WeightG,MountSizeMm,Price")] FPVCameraComponent fPVCameraComponent)
        {
            if (id != fPVCameraComponent.CameraId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fPVCameraComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FPVCameraComponentExists(fPVCameraComponent.CameraId))
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
            return View(fPVCameraComponent);
        }

        // GET: FPVCamera/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fPVCameraComponent = await _context.FPVCameraComponents
                .FirstOrDefaultAsync(m => m.CameraId == id);
            if (fPVCameraComponent == null)
            {
                return NotFound();
            }

            return View(fPVCameraComponent);
        }

        // POST: FPVCamera/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fPVCameraComponent = await _context.FPVCameraComponents.FindAsync(id);
            if (fPVCameraComponent != null)
            {
                _context.FPVCameraComponents.Remove(fPVCameraComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FPVCameraComponentExists(int id)
        {
            return _context.FPVCameraComponents.Any(e => e.CameraId == id);
        }
    }
}
