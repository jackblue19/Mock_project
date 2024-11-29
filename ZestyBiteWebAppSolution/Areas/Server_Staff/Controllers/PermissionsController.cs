using Microsoft.AspNetCore.Mvc;

namespace ZestyBiteSolution.Areas.Server_Staff.Controllers {
    [Area("Server_Staff")] // Đánh dấu controller thuộc Area "Server_Staff"
    public class PermissionsController : Controller {
        // GET: Server_Staff/Permissions
        public IActionResult Permission() {
            return View();
        }
    }
}
