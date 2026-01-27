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
    public class FCController : Controller
    {
        private readonly AppDbContext _context;

        public FCController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FC
        public async Task<IActionResult> Index()
        {
            return View(await _context.FCComponents.ToListAsync());
        }

        // GET: FC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fCComponent = await _context.FCComponents
                .FirstOrDefaultAsync(m => m.FcId == id);
            if (fCComponent == null)
            {
                return NotFound();
            }

            return View(fCComponent);
        }

        // GET: FC/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FcId,Manufacturer,Model,McuProcessor,ImuGyro,MountPatternMm,VoltageInputS,FirmwareSupport,WeightG,Price")] FCComponent fCComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fCComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fCComponent);
        }

        // GET: FC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fCComponent = await _context.FCComponents.FindAsync(id);
            if (fCComponent == null)
            {
                return NotFound();
            }
            return View(fCComponent);
        }

        // POST: FC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FcId,Manufacturer,Model,McuProcessor,ImuGyro,MountPatternMm,VoltageInputS,FirmwareSupport,WeightG,Price")] FCComponent fCComponent)
        {
            if (id != fCComponent.FcId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fCComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FCComponentExists(fCComponent.FcId))
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
            return View(fCComponent);
        }

        // GET: FC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fCComponent = await _context.FCComponents
                .FirstOrDefaultAsync(m => m.FcId == id);
            if (fCComponent == null)
            {
                return NotFound();
            }

            return View(fCComponent);
        }

        // POST: FC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fCComponent = await _context.FCComponents.FindAsync(id);
            if (fCComponent != null)
            {
                _context.FCComponents.Remove(fCComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FCComponentExists(int id)
        {
            return _context.FCComponents.Any(e => e.FcId == id);
        }
    }
}
