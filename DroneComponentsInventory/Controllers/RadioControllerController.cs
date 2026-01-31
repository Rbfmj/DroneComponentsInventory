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
    public class RadioControllerController : Controller
    {
        private readonly AppDbContext _context;

        public RadioControllerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RadioController
        public async Task<IActionResult> Index()
        {
            return View(await _context.RadioControllerComponents.ToListAsync());
        }

        // GET: RadioController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var radioControllerComponent = await _context.RadioControllerComponents
                .FirstOrDefaultAsync(m => m.RadioControllerId == id);
            if (radioControllerComponent == null)
            {
                return NotFound();
            }

            return View(radioControllerComponent);
        }

        // GET: RadioController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RadioController/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RadioControllerId,Manufacturer,Model,ControllerStyle,FrequencyGhz,ProtocolsSupported,MaxChannels,OutputPowerMw,TelemetrySupport,ScreenType,GimbalType,BatteryType,WeightG,FirmwareSupport,Price")] RadioControllerComponent radioControllerComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(radioControllerComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(radioControllerComponent);
        }

        // GET: RadioController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var radioControllerComponent = await _context.RadioControllerComponents.FindAsync(id);
            if (radioControllerComponent == null)
            {
                return NotFound();
            }
            return View(radioControllerComponent);
        }

        // POST: RadioController/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RadioControllerId,Manufacturer,Model,ControllerStyle,FrequencyGhz,ProtocolsSupported,MaxChannels,OutputPowerMw,TelemetrySupport,ScreenType,GimbalType,BatteryType,WeightG,FirmwareSupport,Price")] RadioControllerComponent radioControllerComponent)
        {
            if (id != radioControllerComponent.RadioControllerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(radioControllerComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RadioControllerComponentExists(radioControllerComponent.RadioControllerId))
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
            return View(radioControllerComponent);
        }

        // GET: RadioController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var radioControllerComponent = await _context.RadioControllerComponents
                .FirstOrDefaultAsync(m => m.RadioControllerId == id);
            if (radioControllerComponent == null)
            {
                return NotFound();
            }

            return View(radioControllerComponent);
        }

        // POST: RadioController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var radioControllerComponent = await _context.RadioControllerComponents.FindAsync(id);
            if (radioControllerComponent != null)
            {
                _context.RadioControllerComponents.Remove(radioControllerComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RadioControllerComponentExists(int id)
        {
            return _context.RadioControllerComponents.Any(e => e.RadioControllerId == id);
        }
    }
}
