using Microsoft.AspNetCore.Mvc;

namespace ZestyBiteSolution.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult About() {
            return View();
        }

        public IActionResult Contact() {
            return View();
        }

        public IActionResult Feedback() {
            return View();
        }

        public IActionResult Services() {
            return View();
        }

        public IActionResult Blog() {
            return View();
        }

        public IActionResult Menu() {
            return View();
        }

        public IActionResult Search() {
            return PartialView();
        }

        public IActionResult BookTable() {
            TempData["ShowPopup"] = true;
            return View();
        }
    }
}
