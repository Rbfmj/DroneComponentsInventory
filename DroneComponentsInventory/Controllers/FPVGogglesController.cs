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
    public class FPVGogglesController : Controller
    {
        private readonly AppDbContext _context;

        public FPVGogglesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FPVGoggles
        public async Task<IActionResult> Index()
        {
            return View(await _context.FPVGogglesComponents.ToListAsync());
        }

        // GET: FPVGoggles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fPVGogglesComponent = await _context.FPVGogglesComponents
                .FirstOrDefaultAsync(m => m.FPVGogglesId == id);
            if (fPVGogglesComponent == null)
            {
                return NotFound();
            }

            return View(fPVGogglesComponent);
        }

        // GET: FPVGoggles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FPVGoggles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FPVGogglesId,Manufacturer,Model,VideoSystem,DisplayType,ScreenSizeInch,Resolution,LatencyMs,ReceiverType,DiversityAntennas,WeightGrams,DvrCapability,BatteryLifeHours,PowerInput,IPDAdjustable,Price")] FPVGogglesComponent fPVGogglesComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fPVGogglesComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fPVGogglesComponent);
        }

        // GET: FPVGoggles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fPVGogglesComponent = await _context.FPVGogglesComponents.FindAsync(id);
            if (fPVGogglesComponent == null)
            {
                return NotFound();
            }
            return View(fPVGogglesComponent);
        }

        // POST: FPVGoggles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FPVGogglesId,Manufacturer,Model,VideoSystem,DisplayType,ScreenSizeInch,Resolution,LatencyMs,ReceiverType,DiversityAntennas,WeightGrams,DvrCapability,BatteryLifeHours,PowerInput,IPDAdjustable,Price")] FPVGogglesComponent fPVGogglesComponent)
        {
            if (id != fPVGogglesComponent.FPVGogglesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fPVGogglesComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FPVGogglesComponentExists(fPVGogglesComponent.FPVGogglesId))
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
            return View(fPVGogglesComponent);
        }

        // GET: FPVGoggles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fPVGogglesComponent = await _context.FPVGogglesComponents
                .FirstOrDefaultAsync(m => m.FPVGogglesId == id);
            if (fPVGogglesComponent == null)
            {
                return NotFound();
            }

            return View(fPVGogglesComponent);
        }

        // POST: FPVGoggles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fPVGogglesComponent = await _context.FPVGogglesComponents.FindAsync(id);
            if (fPVGogglesComponent != null)
            {
                _context.FPVGogglesComponents.Remove(fPVGogglesComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FPVGogglesComponentExists(int id)
        {
            return _context.FPVGogglesComponents.Any(e => e.FPVGogglesId == id);
        }
    }
}
