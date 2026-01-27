using Microsoft.AspNetCore.Mvc;

namespace DroneComponentsInventory.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
