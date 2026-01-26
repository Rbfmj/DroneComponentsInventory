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
    public class PropellersController : Controller
    {
        private readonly AppDbContext _context;

        public PropellersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Propellers
        public async Task<IActionResult> Index()
        {
            return View(await _context.PropellersComponents.ToListAsync());
        }

        // GET: Propellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propellersComponent = await _context.PropellersComponents
                .FirstOrDefaultAsync(m => m.PropellerId == id);
            if (propellersComponent == null)
            {
                return NotFound();
            }

            return View(propellersComponent);
        }

        // GET: Propellers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Propellers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PropellerId,Manufacturer,Model,DiameterMm,PitchInch,BladeCount,Material,ShaftDiameterMm,RotationDirection,RecommendedMotorSize,RecommendedMotorKv,FrameClass,WeightG,ColorOptions,IncludedQuantity,Price")] PropellersComponent propellersComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(propellersComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(propellersComponent);
        }

        // GET: Propellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propellersComponent = await _context.PropellersComponents.FindAsync(id);
            if (propellersComponent == null)
            {
                return NotFound();
            }
            return View(propellersComponent);
        }

        // POST: Propellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PropellerId,Manufacturer,Model,DiameterMm,PitchInch,BladeCount,Material,ShaftDiameterMm,RotationDirection,RecommendedMotorSize,RecommendedMotorKv,FrameClass,WeightG,ColorOptions,IncludedQuantity,Price")] PropellersComponent propellersComponent)
        {
            if (id != propellersComponent.PropellerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(propellersComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropellersComponentExists(propellersComponent.PropellerId))
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
            return View(propellersComponent);
        }

        // GET: Propellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propellersComponent = await _context.PropellersComponents
                .FirstOrDefaultAsync(m => m.PropellerId == id);
            if (propellersComponent == null)
            {
                return NotFound();
            }

            return View(propellersComponent);
        }

        // POST: Propellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var propellersComponent = await _context.PropellersComponents.FindAsync(id);
            if (propellersComponent != null)
            {
                _context.PropellersComponents.Remove(propellersComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropellersComponentExists(int id)
        {
            return _context.PropellersComponents.Any(e => e.PropellerId == id);
        }
    }
}
