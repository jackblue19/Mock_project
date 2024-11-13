using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers {
    public class CartController : Controller {
        // GET: Cart
        public IActionResult Index() {
            return View();
        }

        public IActionResult CartPartial() {
            return PartialView();
        }

        public IActionResult ShoppingCart() {
            return View();
        }

        public IActionResult Payment() {
            return View();
        }
    }
}
