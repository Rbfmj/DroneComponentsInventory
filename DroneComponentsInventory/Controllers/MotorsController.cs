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
    public class MotorsController : Controller
    {
        private readonly AppDbContext _context;

        public MotorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Motors
        public async Task<IActionResult> Index()
        {
            return View(await _context.MotorsComponents.ToListAsync());
        }

        // GET: Motors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorsComponent = await _context.MotorsComponents
                .FirstOrDefaultAsync(m => m.MotorId == id);
            if (motorsComponent == null)
            {
                return NotFound();
            }

            return View(motorsComponent);
        }

        // GET: Motors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Motors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MotorId,Manufacturer,Model,StatorSizeMm,Kv,MountPattern,WeightG,RecommendedVoltageS,RecommendedPropInch,MaxThrustG,Price")] MotorsComponent motorsComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(motorsComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(motorsComponent);
        }

        // GET: Motors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorsComponent = await _context.MotorsComponents.FindAsync(id);
            if (motorsComponent == null)
            {
                return NotFound();
            }
            return View(motorsComponent);
        }

        // POST: Motors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MotorId,Manufacturer,Model,StatorSizeMm,Kv,MountPattern,WeightG,RecommendedVoltageS,RecommendedPropInch,MaxThrustG,Price")] MotorsComponent motorsComponent)
        {
            if (id != motorsComponent.MotorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(motorsComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotorsComponentExists(motorsComponent.MotorId))
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
            return View(motorsComponent);
        }

        // GET: Motors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorsComponent = await _context.MotorsComponents
                .FirstOrDefaultAsync(m => m.MotorId == id);
            if (motorsComponent == null)
            {
                return NotFound();
            }

            return View(motorsComponent);
        }

        // POST: Motors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var motorsComponent = await _context.MotorsComponents.FindAsync(id);
            if (motorsComponent != null)
            {
                _context.MotorsComponents.Remove(motorsComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotorsComponentExists(int id)
        {
            return _context.MotorsComponents.Any(e => e.MotorId == id);
        }
    }
}
