using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models;
using ZestyBiteWebAppSolution.Models.ViewModel;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

public class CartController : Controller {
    private const string CartSessionKey = "Cart";
    private readonly ZestyBiteContext _context;
    private readonly IVnPayService _vnPayService;
    private readonly IBillRepository _billRepository;

    public CartController(ZestyBiteContext context, IVnPayService vnPayService, IBillRepository billRepository) {
        _context = context;
        _vnPayService = vnPayService;
        _billRepository = billRepository;
    }

    // Display Cart
    public IActionResult Cart() {
        var cart = GetCheckout();

        ViewBag.TotalItems = cart.Items.Sum(i => i.Quantity); 
        ViewBag.TotalAmount = cart.Items.Sum(i => i.Quantity * i.Price); 

        return View(cart);
    }
    public IActionResult Checkout() { 
        return View();
    }

    [HttpPost]
    [Route("api/Cart/Payment")]
    public async Task<IActionResult> Payment(VnPaymentRequestModel paymentRequest) {
        var username = HttpContext.Request.Cookies["username"];

        if (string.IsNullOrEmpty(username)) {
            return Unauthorized(new { message = "User is not logged in." });
        }

        var cart = await _billRepository.GetBillAsync(int.Parse(username));
        if (cart == null) {
            return NotFound(new { message = "Cart not found." });
        }

        if (ModelState.IsValid && paymentRequest.PaymentMethod == "Payment") {
            var acc = await _billRepository.GetNameById(int.Parse(username));
            var vnPayModel = new VnPaymentRequestModel {
                Amount = cart.TotalCost,
                CreatedDate = DateTime.Now,
                Description = $"{acc.Name} {acc.PhoneNumber}",
            };

            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
            return Ok(new { message = "Payment successful.", paymentUrl });
        }

        return BadRequest(new { message = "Invalid data." });
    }

    private CheckoutDTO GetCheckout() {
        var cart = HttpContext.Session.GetObjectFromJson<CheckoutDTO>(CartSessionKey);
        if (cart == null) {
            cart = new CheckoutDTO {
                Items = new List<CheckoutItemDTO>() // Initialize empty list
            };
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return cart;
    }

    public IActionResult AddToCart(int itemId) {
        var cart = GetCheckout();

        // Fetch item từ database
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
            return NotFound(new { message = "Item not found." });
        }

        // Thêm hoặc cập nhật số lượng item trong giỏ
        var existingItem = cart.Items.FirstOrDefault(i => i.ItemId == item.ItemId);
        if (existingItem != null) {
            existingItem.Quantity++;
        } else {
            cart.Items.Add(item);
        }

        // Lưu lại Session
        HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

        var totalItems = cart.Items.Sum(i => i.Quantity);

        return RedirectToAction("Index", "Home", new { cartTotalItems = totalItems });

    }

    public IActionResult RemoveFromCart(int itemId) {
        var cart = GetCheckout();
        var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item != null) {
            cart.Items.Remove(item);
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return View("", cart);
    }

    [HttpPost]
    public IActionResult UpdateCart(int itemId, int quantity) {
        var cart = GetCheckout();
        var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item != null) {
            item.Quantity = quantity;
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return RedirectToAction("Cart");
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
            TempData["Message"] = $"Payment failed: {response?.VnPayResponseCode}";
            return RedirectToAction("PaymentFail");
        }

        TempData["Message"] = "Payment successful.";
        return RedirectToAction("PaymentSuccess");
    }
}