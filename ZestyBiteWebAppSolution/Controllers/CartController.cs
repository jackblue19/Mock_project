using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models;
namespace ZestyBiteWebAppSolution.Controllers {
    public class CartController : Controller {
        private const string CartSessionKey = "ShoppingCart";
        private readonly ZestyBiteContext _context;
        public CartController(ZestyBiteContext context) {
            _context = context;
        }
        private ShoppingCartDTO GetShoppingCart() {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCartDTO>("ShoppingCart");
            if (cart == null) {
                cart = new ShoppingCartDTO();
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
            }
            return cart;
        }

        public IActionResult AddToCart(int itemId) {
            if (!User.Identity.IsAuthenticated) {
                return RedirectToAction("Register", "Account"); // Chuyển hướng đến trang đăng ký
            }
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCartDTO>("ShoppingCart") ?? new ShoppingCartDTO();

            var item = _context.Items
                               .Where(i => i.ItemId == itemId)
                               .Select(i => new ShoppingCartItemDTO {
                                   ItemId = i.ItemId,
                                   Name = i.ItemName,
                                   Price = i.SuggestedPrice,
                                   ImageUrl = i.ItemImage,
                                   Quantity = 1
                               })
                               .FirstOrDefault();

            if (item == null) {
                return NotFound();
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ItemId == item.ItemId);
            if (existingItem != null) {
                existingItem.Quantity++;
            } else {
                cart.Items.Add(item);
            }

            HttpContext.Session.SetObjectAsJson("ShoppingCart", cart);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShoppingCart() {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCartDTO>("ShoppingCart") ?? new ShoppingCartDTO();

            var totalItems = cart.Items.Sum(i => i.Quantity);
            var totalAmount = cart.Items.Sum(i => i.Quantity * i.Price);

            ViewBag.TotalItems = totalItems;
            ViewBag.TotalAmount = totalAmount;

            return View(cart);
            
        }


        public IActionResult RemoveFromCart(int itemId) {
            var cart = GetShoppingCart();
            var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null) {
                cart.Items.Remove(item);
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

                var totalItems = cart.Items.Sum(i => i.Quantity);
                var totalAmount = cart.Items.Sum(i => i.Quantity * i.Price);
                ViewBag.TotalItems = totalItems;
                ViewBag.TotalAmount = totalAmount;
            }

            return RedirectToAction("ShoppingCart");
        }

        public IActionResult UpdateCart(int itemId, int quantity) {
            var cart = GetShoppingCart();
            var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null) {
                item.Quantity = quantity;
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

                var totalItems = cart.Items.Sum(i => i.Quantity);
                var totalAmount = cart.Items.Sum(i => i.Quantity * i.Price);
                ViewBag.TotalItems = totalItems;
                ViewBag.TotalAmount = totalAmount;
            }
            return RedirectToAction("ShoppingCart");
        }
        public IActionResult Bill() {
            return View();
        }
    }
}
