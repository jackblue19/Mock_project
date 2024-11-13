using Microsoft.AspNetCore.Mvc;

namespace ZestyBiteSolution.Areas.Server_Staff.Controllers {
    [Area("Server_Staff")] // Đánh dấu đây là controller thuộc Area "Server_Staff"
    public class HomeController : Controller {
        // Trang Index
        public IActionResult Index() {
            // Kiểm tra session "service_staff"
            if (HttpContext.Session.GetString("service_staff") == null) {
                return RedirectToAction("Permission", "Permission");
            } else {
                return View();
            }
        }

        // Trang Check_Server
        public IActionResult Check_Server() {
            return View();
        }

        // Trang Server_Table
        public IActionResult Server_Table() {
            return View();
        }
    }
}