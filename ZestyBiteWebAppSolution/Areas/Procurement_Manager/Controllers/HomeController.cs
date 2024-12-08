using Microsoft.AspNetCore.Mvc;

namespace ZestyBiteSolution.Areas.Procurement_Manager.Controllers {
    [Area("Procurement_Manager")] 
    public class HomeController : Controller {
        // Action cho trang Supply
        public IActionResult Supply() {
            return View();
        }

        // Action cho trang Index
        public IActionResult Index() {
            return View();
        }
    }
}
