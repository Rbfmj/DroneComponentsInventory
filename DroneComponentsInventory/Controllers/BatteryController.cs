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
    public class BatteryController : Controller
    {
        private readonly AppDbContext _context;

        public BatteryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Battery
        public async Task<IActionResult> Index()
        {
            return View(await _context.BatteryComponents.ToListAsync());
        }

        // GET: Battery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batteryComponent = await _context.BatteryComponents
                .FirstOrDefaultAsync(m => m.BatteryId == id);
            if (batteryComponent == null)
            {
                return NotFound();
            }

            return View(batteryComponent);
        }

        // GET: Battery/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Battery/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatteryId,Manufacturer,Model,CellCountS,NominalVoltageV,CapacityMah,DischargeRateC,BurstRateC,DischargeConnector,BalanceConnector,Chemistry,WeightG,LengthMm,WidthMm,HeightMm,Price")] BatteryComponent batteryComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batteryComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(batteryComponent);
        }

        // GET: Battery/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batteryComponent = await _context.BatteryComponents.FindAsync(id);
            if (batteryComponent == null)
            {
                return NotFound();
            }
            return View(batteryComponent);
        }

        // POST: Battery/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatteryId,Manufacturer,Model,CellCountS,NominalVoltageV,CapacityMah,DischargeRateC,BurstRateC,DischargeConnector,BalanceConnector,Chemistry,WeightG,LengthMm,WidthMm,HeightMm,Price")] BatteryComponent batteryComponent)
        {
            if (id != batteryComponent.BatteryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batteryComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatteryComponentExists(batteryComponent.BatteryId))
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
            return View(batteryComponent);
        }

        // GET: Battery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batteryComponent = await _context.BatteryComponents
                .FirstOrDefaultAsync(m => m.BatteryId == id);
            if (batteryComponent == null)
            {
                return NotFound();
            }

            return View(batteryComponent);
        }

        // POST: Battery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batteryComponent = await _context.BatteryComponents.FindAsync(id);
            if (batteryComponent != null)
            {
                _context.BatteryComponents.Remove(batteryComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatteryComponentExists(int id)
        {
            return _context.BatteryComponents.Any(e => e.BatteryId == id);
        }
    }
}
