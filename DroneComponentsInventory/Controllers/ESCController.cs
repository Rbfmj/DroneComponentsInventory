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
    public class ESCController : Controller
    {
        private readonly AppDbContext _context;

        public ESCController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ESC
        public async Task<IActionResult> Index()
        {
            return View(await _context.ESCComponents.ToListAsync());
        }

        // GET: ESC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSCComponent = await _context.ESCComponents
                .FirstOrDefaultAsync(m => m.EscId == id);
            if (eSCComponent == null)
            {
                return NotFound();
            }

            return View(eSCComponent);
        }

        // GET: ESC/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ESC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EscId,Manufacturer,Model,EscType,ContinuousCurrentA,VoltageInputS,MountPatternMm,SupportedProtocols,WeightG,Price")] ESCComponent eSCComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eSCComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eSCComponent);
        }

        // GET: ESC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSCComponent = await _context.ESCComponents.FindAsync(id);
            if (eSCComponent == null)
            {
                return NotFound();
            }
            return View(eSCComponent);
        }

        // POST: ESC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EscId,Manufacturer,Model,EscType,ContinuousCurrentA,VoltageInputS,MountPatternMm,SupportedProtocols,WeightG,Price")] ESCComponent eSCComponent)
        {
            if (id != eSCComponent.EscId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSCComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESCComponentExists(eSCComponent.EscId))
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
            return View(eSCComponent);
        }

        // GET: ESC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSCComponent = await _context.ESCComponents
                .FirstOrDefaultAsync(m => m.EscId == id);
            if (eSCComponent == null)
            {
                return NotFound();
            }

            return View(eSCComponent);
        }

        // POST: ESC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSCComponent = await _context.ESCComponents.FindAsync(id);
            if (eSCComponent != null)
            {
                _context.ESCComponents.Remove(eSCComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ESCComponentExists(int id)
        {
            return _context.ESCComponents.Any(e => e.EscId == id);
        }
    }
}
