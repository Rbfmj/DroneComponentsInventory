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
    public class FrameController : Controller
    {
        private readonly AppDbContext _context;

        public FrameController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Frame
        public async Task<IActionResult> Index()
        {
            return View(await _context.FrameComponents.ToListAsync());
        }

        // GET: Frame/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var frameComponent = await _context.FrameComponents
                .FirstOrDefaultAsync(m => m.FrameId == id);
            if (frameComponent == null)
            {
                return NotFound();
            }

            return View(frameComponent);
        }

        // GET: Frame/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Frame/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FrameId,Manufacturer,Model,WheelbaseMm,MaxPropInch,Geometry,Material,FrameWeightG,ArmThicknessMm,MotorMountPattern,FcMountPattern,MaxStackHeightMm,Price")] FrameComponent frameComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(frameComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(frameComponent);
        }

        // GET: Frame/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var frameComponent = await _context.FrameComponents.FindAsync(id);
            if (frameComponent == null)
            {
                return NotFound();
            }
            return View(frameComponent);
        }

        // POST: Frame/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FrameId,Manufacturer,Model,WheelbaseMm,MaxPropInch,Geometry,Material,FrameWeightG,ArmThicknessMm,MotorMountPattern,FcMountPattern,MaxStackHeightMm,Price")] FrameComponent frameComponent)
        {
            if (id != frameComponent.FrameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(frameComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FrameComponentExists(frameComponent.FrameId))
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
            return View(frameComponent);
        }

        // GET: Frame/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var frameComponent = await _context.FrameComponents
                .FirstOrDefaultAsync(m => m.FrameId == id);
            if (frameComponent == null)
            {
                return NotFound();
            }

            return View(frameComponent);
        }

        // POST: Frame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var frameComponent = await _context.FrameComponents.FindAsync(id);
            if (frameComponent != null)
            {
                _context.FrameComponents.Remove(frameComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FrameComponentExists(int id)
        {
            return _context.FrameComponents.Any(e => e.FrameId == id);
        }
    }
}
