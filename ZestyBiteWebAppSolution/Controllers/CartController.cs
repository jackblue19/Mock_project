using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.ViewModel;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

using ZestyBiteWebAppSolution.Models;
using ZestyBiteWebAppSolution.Models.ViewModel;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers {
    [AllowAnonymous]
    [AllowAnonymous]
    public class CartController : Controller {
        private const string CartSessionKey = "Checkout";
        private readonly ZestyBiteContext _context;
        private readonly IVnPayService _vnPayService;
        private readonly IBillRepository _billRepository;

        public CartController(ZestyBiteContext context, IVnPayService vnPayService, IBillRepository billRepository) {
        private readonly IVnPayService _vpnPayService;
        private readonly IBillRepository _billRepository;
        public CartController(ZestyBiteContext context, IVnPayService vnPayService, IBillRepository billRepository) {
            _context = context;
            _vnPayService = vnPayService;
            _billRepository = billRepository;
        }

        public IActionResult Checkout() {
            var cart = GetCheckout();
            return View(cart);
        }

        [HttpPost]
        [Route("api/Cart/Payment")]
        public async Task<IActionResult> Payment([FromBody] VnPaymentRequestModel paymentRequest) {
            // Lấy username từ cookie
            var username = HttpContext.Request.Cookies["username"];

            if (string.IsNullOrEmpty(username)) {
                // Trả về lỗi nếu người dùng chưa đăng nhập
                return Unauthorized(new { message = "Người dùng chưa đăng nhập." });
            }

            var cart = await _billRepository.GetBillAsync(int.Parse(username)); // Lấy thông tin giỏ hàng
            if (cart == null) {
                return NotFound(new { message = "Không tìm thấy giỏ hàng." });
            }

            if (ModelState.IsValid) {
                if (paymentRequest.PaymentMethod == "Payment") {
                    var acc = await _billRepository.GetNameById(int.Parse(username));
                    var vnPayModel = new VnPaymentRequestModel {
                        Amount = cart.TotalCost,
                        CreatedDate = DateTime.Now,
                        Description = $"{acc.Name} {acc.PhoneNumber}",
                    };

                    var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
                    return Ok(new { message = "Thanh toán thành công.", paymentUrl });
                }
            }

            return BadRequest(new { message = "Dữ liệu không hợp lệ." });
        }

        private CheckoutDTO GetCheckout() {
            var cart = HttpContext.Session.GetObjectFromJson<CheckoutDTO>(CartSessionKey);
            _vpnPayService = vnPayService;
            _billRepository = billRepository;
        }
        public IActionResult Checkout() {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(string payment = "VnPay", int usn) {
            // Retrieve the cart
            var cart = await _billRepository.GetBillAsync(usn);
            var table = cart.Table;


            // Ensure Items is initialized
            table.TableDetails

            if (ModelState.IsValid) {
                if (payment == "VnPay") {
                    var acc = await _billRepository.GetNameById(usn);
                    var vnPayModel = new VnPaymentRequestModel {
                        Amount = cart.TotalCost,
                        CreatedDate = DateTime.Now,
                        Description = $"{acc.Name} {acc.PhoneNumber}",
                    };
                    return Redirect(_vpnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                }
            }

            // Return the model to the view
            return View(cart);
        }



        private ShoppingCartDTO GetShoppingCart() {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCartDTO>("ShoppingCart");
            if (cart == null) {
                cart = new CheckoutDTO();
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cart); // Save the new cart to the session
            }
            return cart;
        }

        public IActionResult AddToCart(int itemId) {
            var cart = GetCheckout();
            if (!User.Identity.IsAuthenticated) {
                return RedirectToAction("Register", "Account");
            }
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCartDTO>("ShoppingCart") ?? new ShoppingCartDTO();

            var item = _context.Items
                               .Where(i => i.ItemId == itemId)
                               .Select(i => new CheckoutItemDTO {
                                   ItemId = i.ItemId,
                                   Name = i.ItemName,
                                   Price = i.SuggestedPrice,
                                   ImageUrl = i.ItemImage,
                                   Quantity = 1
                               })
                               .FirstOrDefault();

            if (item == null) {
                return NotFound(); // Handle item not found
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ItemId == item.ItemId);
            if (existingItem != null) {
                existingItem.Quantity++;
            } else {
                cart.Items.Add(item);
            }

            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart); // Save updated cart to session
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShoppingCart() {
            var cart = GetCheckout();

            ViewBag.TotalItems = cart.Items.Sum(i => i.Quantity);
            ViewBag.TotalAmount = cart.Items.Sum(i => i.Quantity * i.Price);

            return View(cart);

        }

        public IActionResult RemoveFromCart(int itemId) {
            var cart = GetCheckout();
            var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null) {
                cart.Items.Remove(item);
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cart); // Save updated cart to session
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

                var totalItems = cart.Items.Sum(i => i.Quantity);
                var totalAmount = cart.Items.Sum(i => i.Quantity * i.Price);
                ViewBag.TotalItems = totalItems;
                ViewBag.TotalAmount = totalAmount;

                var totalItems = cart.Items.Sum(i => i.Quantity);
                var totalAmount = cart.Items.Sum(i => i.Quantity * i.Price);
                ViewBag.TotalItems = totalItems;
                ViewBag.TotalAmount = totalAmount;
            }

            return RedirectToAction("Checkout");
            return RedirectToAction("ShoppingCart");
            return RedirectToAction("ShoppingCart");
        }

        public IActionResult UpdateCart(int itemId, int quantity) {
            var cart = GetCheckout();
            var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null) {
                item.Quantity = quantity;
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cart); // Save updated cart to session
            }
            return RedirectToAction("Checkout");
        }

        public IActionResult Bill() {
            return View();
        }

        [Authorize]
        public IActionResult PaymentFail() { 
            return View();
        }
        [Authorize]
        public IActionResult PaymentSuccess() {
            return View("Success");
        }

        [Authorize]
        public IActionResult PaymentCallBack() {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00") {
                TempData["Message"] = $"Fail Payment VnPay : {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }
            TempData["Message"] = "Success ";
            return RedirectToAction("PaymentSuccess");
        }

        [Authorize]
        public IActionResult PaymentCallBack() {
            return View();
        }
    }
}