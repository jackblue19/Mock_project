using Microsoft.AspNetCore.Mvc;

namespace ZestyBiteSolution.Areas.Food_Runner.Controllers {
    [Area("Food_Runner")]
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
