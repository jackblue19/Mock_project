using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.ViewModel;
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

    public async Task<IActionResult> Cart() {
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

        ViewBag.TotalItems = 0;
        ViewBag.TotalAmount = 0;


        return View("Cart", cart);

    }

    // Display Cart
    [HttpPost]
    [Route("api/cart/saving")]
    public async Task<IActionResult> CartAPI() {
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
        //ViewBag.Items = cart.Items; // Ensure this is set properly
        ViewBag.TotalItems = cart.TotalItems;
        ViewBag.TotalAmount = cart.TotalPrice;

        var savedValue = HttpContext.Session.GetString("save");
        bool flag = true;
        var uflag = HttpContext.Session.GetString("uflag");
        // Check if the paymentFlag is 1 or 0
        if (savedValue == uflag) {
            flag = true;  // Do not process payment logic
        } else {
            // Process payment if flag is 1
            var itemQuantityMap = cart.Items.ToDictionary(i => i.ItemId, i => i.Quantity);

            // Call ToPayment service
            int billId;
            try {
                billId = _tableDetailService.ToPayment(itemQuantityMap, acc, CartSessionKey, HttpContext).Result;
            } catch {
                return BadRequest("del oonr rooannadnanodas");
            }
            if (billId == 0) {
                flag = false;
            } else {
                flag = true;
            }
        }

        if (flag) {
            ViewBag.TotalItems = 0;
            ViewBag.TotalAmount = 0;
            //HttpContext.Session.SetString("billId", billId);
            return Ok(); // billId
        } else {
            ViewBag.ErrorMessage = "An error occurred during payment processing.";
            return BadRequest("Failed to load information");
        }
    }

    [HttpPost("api/Cart/Change")]
    public IActionResult SetUFlag() {
        // Set the session key "uflag" to 1
        HttpContext.Session.SetString("uflag", "1");

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Checkout(string payment = "VnPay") {
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
        }
        return View();

    }
    // In CartController
    [HttpPost]
    [Route("api/bill/checkout")]
    public async Task<IActionResult> CheckoutAPI() {
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

            int billId = await _billRepository.GetLatestBillIdByUsn(usn);

            if (true) {
                var vnPayModel = new VnPaymentRequestModel {
                    BillId = billId,
                    CreatedDate = DateTime.Now,
                    Description = $"{acc.Name}",
                    Amount = 1000,
                    PaymentMethod = "VnPay",
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                //return RedirectToAction("Index", "Home");
            }

        }
        return BadRequest("");

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
                    //TableId =
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
        HttpContext.Session.SetString("save", "0");

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