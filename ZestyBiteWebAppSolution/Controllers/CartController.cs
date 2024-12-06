using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

public class CartController : Controller {
    private const string CartSessionKey = "Cart";
    private readonly ZestyBiteContext _context;
    private readonly IVnPayService _vnPayService;
    private readonly IBillRepository _billRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITableRepository _tableRepository;
    private readonly ITableDetailRepository _tableDetailRepository;
    private readonly ITableDetailService _tableDetailService;
    private readonly IBillService _billService;
    private readonly IAccountService _accountService;
    public CartController(ZestyBiteContext context,
                            IVnPayService vnPayService,
                            IBillRepository billRepository,
                            IAccountRepository acc,
                            ITableRepository tb,
                            ITableDetailRepository tbd,
                            ITableDetailService tableDetailService,
                            IBillService billService,
                            IAccountService accountService) {
        _context = context;
        _vnPayService = vnPayService;
        _billRepository = billRepository;
        _accountRepository = acc;
        _tableRepository = tb;
        _tableDetailRepository = tbd;
        _tableDetailService = tableDetailService;
        _billService = billService;
        _accountService = accountService;
    }

    // Display Cart
    public async Task<IActionResult> Cart() {
        // Retrieve username from session or cookies
        var usn = HttpContext.Session.GetString("username") ?? Request.Cookies["username"];
        if (string.IsNullOrEmpty(usn)) {
            return RedirectToAction("Login", "Account");
        }

        // Fetch the account associated with the username
        var acc = await _accountRepository.GetAccountByUsnAsync(usn);
        if (acc == null) {
            ViewBag.ErrorMessage = "Account not found.";
            return BadRequest("Account not found.");
        }

        // Get the cart
        var cart = GetCheckout();
        if (cart == null || !cart.Items.Any()) {
            ViewBag.ErrorMessage = "Your cart is empty!";
            return View(cart);
        }

        // Populate ViewBag with cart details
        ViewBag.TotalItems = cart.TotalItems;
        ViewBag.TotalAmount = cart.TotalPrice;
        TempData["totalItems"] = cart.TotalItems;

        var itemQuantityMap = cart.Items.ToDictionary(i => i.ItemId, i => i.Quantity);

        // Call ToPayment service
        var result = await _tableDetailService.ToPayment(itemQuantityMap, acc, CartSessionKey, HttpContext); //cần gọi ra id của table 

        if (result) {
            ViewBag.TotalItems = 0;
            ViewBag.TotalAmount = 0;
            return View("Cart", cart);
        } else {
            ViewBag.ErrorMessage = "An error occurred during payment processing.";
            // Return an error message view or BadRequest
            return BadRequest("Failed to load information");
        }
    }

    // In CartController
    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutDTO model, string payment = "COD") {
        if (ModelState.IsValid) {
            var usn = HttpContext.Session.GetString("username") ?? Request.Cookies["username"];
            if (string.IsNullOrEmpty(usn)) {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the account associated with the username
            var acc = await _accountRepository.GetAccountByUsnAsync(usn);
            if (acc == null) {
                ViewBag.ErrorMessage = "Account not found.";
                return BadRequest("Account not found.");
            }

            // Get the cart
            var cart = GetCheckout();
            if (cart == null || !cart.Items.Any()) {
                ViewBag.ErrorMessage = "Your cart is empty!";
                return View(cart);
            }

            // Populate ViewBag with cart details
            ViewBag.TotalItems = cart.TotalItems;
            ViewBag.TotalAmount = cart.TotalPrice;

            // Prepare the CheckoutDTO model for the view
            var checkoutDTO = new CheckoutDTO {
                Items = cart.Items,
                TotalAmount = cart.TotalPrice,
                TableId = cart.TableId,
            };

            await _billRepository.CreateAsync(20);


        }
        return View();

    }

    private CheckoutDTO GetCheckout() {
        // Retrieve cart from session
        var cart = HttpContext.Session.GetObjectFromJson<CheckoutDTO>(CartSessionKey);

        if (cart == null) {
            // Check for a cookie-based cart as a fallback
            if (Request.Cookies.TryGetValue(CartSessionKey, out var cookieCart)) {
            } else {
                cart = new CheckoutDTO {
                    Items = new List<CheckoutItemDTO>(),
                    TotalAmount = 0,
                };
            }

            // Save cart back to session
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }

        return cart;
    }

    public IActionResult AddToCart(int itemId) {
        var cart = GetCheckout();

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

        // Add or update item quantity in cart
        var existingItem = cart.Items.FirstOrDefault(i => i.ItemId == item.ItemId);
        if (existingItem != null) {
            existingItem.Quantity++;
        } else {
            cart.Items.Add(item);
        }

        // Save cart to session
        HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

        // Update the total items in TempData
        TempData["totalItems"] = cart.Items.Sum(i => i.Quantity);

        // Redirect to Home page
        return RedirectToAction("Index", "Home");
    }

    public IActionResult RemoveFromCart(int itemId) {
        var cart = GetCheckout();
        var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item != null) {
            cart.Items.Remove(item);
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult UpdateCart(int itemId, int quantity) {
        var cart = GetCheckout();
        var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item != null && quantity > 0) {
            item.Quantity = quantity;
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        } else if (quantity <= 0) {
            cart.Items.Remove(item);
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return RedirectToAction("Cart");
    }

    [Authorize]
    public IActionResult PaymentFail() {
        return View("PaymentFail");
    }

    [Authorize]
    public IActionResult PaymentSuccess() {
        return View("PaymentSuccess");
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