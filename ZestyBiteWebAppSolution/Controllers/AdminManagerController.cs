using Microsoft.AspNetCore.Mvc;

namespace ZestyBiteWebAppSolution.Controllers {
    public class AdminManagerController : Controller {
        public IActionResult Index() {
            string? userRole = HttpContext.Session.GetString("RoleDescription");

            if (userRole != "Manager") {
                return RedirectToAction("Index", "Home");  
            }
            return View();
        }
    }
}
