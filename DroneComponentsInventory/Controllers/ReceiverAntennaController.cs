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
    public class ReceiverAntennaController : Controller
    {
        private readonly AppDbContext _context;

        public ReceiverAntennaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ReceiverAntenna
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReceiverAntennaComponents.ToListAsync());
        }

        // GET: ReceiverAntenna/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiverAntennaComponent = await _context.ReceiverAntennaComponents
                .FirstOrDefaultAsync(m => m.ReceiverAntennaId == id);
            if (receiverAntennaComponent == null)
            {
                return NotFound();
            }

            return View(receiverAntennaComponent);
        }

        // GET: ReceiverAntenna/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReceiverAntenna/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceiverAntennaId,Manufacturer,Model,FrequencyBand,Polarization,Connector,ConnectorGender,GainDbi,RadiationPattern,CompatibleReceivers,MountType,CableLengthMm,WeightG,LengthMm,Price")] ReceiverAntennaComponent receiverAntennaComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receiverAntennaComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receiverAntennaComponent);
        }

        // GET: ReceiverAntenna/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiverAntennaComponent = await _context.ReceiverAntennaComponents.FindAsync(id);
            if (receiverAntennaComponent == null)
            {
                return NotFound();
            }
            return View(receiverAntennaComponent);
        }

        // POST: ReceiverAntenna/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReceiverAntennaId,Manufacturer,Model,FrequencyBand,Polarization,Connector,ConnectorGender,GainDbi,RadiationPattern,CompatibleReceivers,MountType,CableLengthMm,WeightG,LengthMm,Price")] ReceiverAntennaComponent receiverAntennaComponent)
        {
            if (id != receiverAntennaComponent.ReceiverAntennaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiverAntennaComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiverAntennaComponentExists(receiverAntennaComponent.ReceiverAntennaId))
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
            return View(receiverAntennaComponent);
        }

        // GET: ReceiverAntenna/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiverAntennaComponent = await _context.ReceiverAntennaComponents
                .FirstOrDefaultAsync(m => m.ReceiverAntennaId == id);
            if (receiverAntennaComponent == null)
            {
                return NotFound();
            }

            return View(receiverAntennaComponent);
        }

        // POST: ReceiverAntenna/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receiverAntennaComponent = await _context.ReceiverAntennaComponents.FindAsync(id);
            if (receiverAntennaComponent != null)
            {
                _context.ReceiverAntennaComponents.Remove(receiverAntennaComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiverAntennaComponentExists(int id)
        {
            return _context.ReceiverAntennaComponents.Any(e => e.ReceiverAntennaId == id);
        }
    }
}
