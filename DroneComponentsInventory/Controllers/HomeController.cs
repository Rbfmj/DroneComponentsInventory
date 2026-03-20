using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DroneComponentsInventory.Models;

namespace DroneComponentsInventory.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var builds = await _context.DroneBuilds
                .AsNoTracking()
                .Include(b => b.Frame)
                .Include(b => b.Motor)
                .Include(b => b.Battery)
                .Include(b => b.Fc)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(builds);
        }
    }
}
