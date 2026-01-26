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
    public class ReceiverController : Controller
    {
        private readonly AppDbContext _context;

        public ReceiverController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Receiver
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReceiverComponents.ToListAsync());
        }

        // GET: Receiver/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiverComponent = await _context.ReceiverComponents
                .FirstOrDefaultAsync(m => m.ReceiverId == id);
            if (receiverComponent == null)
            {
                return NotFound();
            }

            return View(receiverComponent);
        }

        // GET: Receiver/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Receiver/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceiverId,Manufacturer,Model,Protocol,ModulationProtocol,FrequencyBand,Channels,TelemetrySupport,AntennaCount,AntennaType,RangeKm,VoltageInputV,OutputSignal,FailsafeSupport,BindingMethod,IntendedUse,WeightG,LengthMm,WidthMm,HeightMm,MountingType,Price")] ReceiverComponent receiverComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receiverComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receiverComponent);
        }

        // GET: Receiver/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiverComponent = await _context.ReceiverComponents.FindAsync(id);
            if (receiverComponent == null)
            {
                return NotFound();
            }
            return View(receiverComponent);
        }

        // POST: Receiver/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReceiverId,Manufacturer,Model,Protocol,ModulationProtocol,FrequencyBand,Channels,TelemetrySupport,AntennaCount,AntennaType,RangeKm,VoltageInputV,OutputSignal,FailsafeSupport,BindingMethod,IntendedUse,WeightG,LengthMm,WidthMm,HeightMm,MountingType,Price")] ReceiverComponent receiverComponent)
        {
            if (id != receiverComponent.ReceiverId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiverComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiverComponentExists(receiverComponent.ReceiverId))
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
            return View(receiverComponent);
        }

        // GET: Receiver/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiverComponent = await _context.ReceiverComponents
                .FirstOrDefaultAsync(m => m.ReceiverId == id);
            if (receiverComponent == null)
            {
                return NotFound();
            }

            return View(receiverComponent);
        }

        // POST: Receiver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receiverComponent = await _context.ReceiverComponents.FindAsync(id);
            if (receiverComponent != null)
            {
                _context.ReceiverComponents.Remove(receiverComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiverComponentExists(int id)
        {
            return _context.ReceiverComponents.Any(e => e.ReceiverId == id);
        }
    }
}
