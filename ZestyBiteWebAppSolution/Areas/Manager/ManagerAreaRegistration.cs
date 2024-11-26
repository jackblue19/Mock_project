using Microsoft.AspNetCore.Mvc;

namespace ZestyBiteSolution.Areas.Manager.Controllers {
    [Area("Manager")] // Đánh dấu đây là controller thuộc Area "Manager"
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
