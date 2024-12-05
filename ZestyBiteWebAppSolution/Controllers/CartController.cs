using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.ViewModel;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

public class CartController : Controller
{
    private const string CartSessionKey = "Cart";
    private readonly ZestyBiteContext _context;
    private readonly IVnPayService _vnPayService;
    private readonly IBillRepository _billRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITableRepository _tableRepository;
    private readonly ITableDetailRepository _tableDetailRepository;
    private readonly ITableDetailService _tableDetailService;
    public CartController(ZestyBiteContext context,
                            IVnPayService vnPayService,
                            IBillRepository billRepository,
                            IAccountRepository acc,
                            ITableRepository tb,
                            ITableDetailRepository tbd,
                            ITableDetailService tableDetailService)
    {
        _context = context;
        _vnPayService = vnPayService;
        _billRepository = billRepository;
        _accountRepository = acc;
        _tableRepository = tb;
        _tableDetailRepository = tbd;
        _tableDetailService = tableDetailService;
    }

    // Display Cart
    /*
    public async Task<IActionResult> Cart()
    {
        var usn = HttpContext.Session.GetString("username") ?? Request.Cookies["username"];
        if (string.IsNullOrEmpty(usn))
        {
            return RedirectToAction("Login", "Account");
        }

        var acc = await _accountRepository.GetAccountByUsnAsync(usn);
        if (acc == null)
        {
            return View("Error", new { message = "Account not found." });
        }

        try
        {
            var cart = GetCheckout();
            if (cart == null || !cart.Items.Any())
            {
                ViewBag.ErrorMessage = "Your cart is empty!";
                TempData["totalItems"] = 0;
                return View(cart);
            }

            ViewBag.TotalItems = cart.TotalItems;
            ViewBag.TotalAmount = cart.TotalPrice;
            TempData["totalItems"] = cart.TotalItems;
            Console.WriteLine("beforeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
            // Prepare the itemQuantityMap for payment service
            var itemQuantityMap = cart.Items.ToDictionary(i => i.ItemId, i => i.Quantity);
            Console.WriteLine("midddddddddddddddddddddddddd");
            // Call ToPayment service method
            var result = await _tableDetailService.ToPayment(itemQuantityMap, acc, CartSessionKey, HttpContext);
            Console.WriteLine("afterrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
            if (result is OkResult)
            {
                Console.WriteLine("beginnnnnnnnnnnnnnnnnnnnn");
                // You can return a success view or redirect after the payment process
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                Console.WriteLine("enddddddddddddddddddd");
                // Handle error case
                ViewBag.ErrorMessage = "An error occurred during payment processing.";
                return View("Error");
            }
        }
        catch (Exception ex)
        {
            return View("Error", new { message = "An error occurred while loading the cart.", details = ex.Message });
        }
    }
*/
    public async Task<IActionResult> Cart()
    {
        // Retrieve username from session or cookies
        var usn = HttpContext.Session.GetString("username") ?? Request.Cookies["username"];
        if (string.IsNullOrEmpty(usn))
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch the account associated with the username
        var acc = await _accountRepository.GetAccountByUsnAsync(usn);
        if (acc == null)
        {
            ViewBag.ErrorMessage = "Account not found.";
            // return View("Error");
            return BadRequest("log acc diiiiiiiiiiii");
        }

        try
        {
            // Get the cart
            var cart = GetCheckout();
            if (cart == null || !cart.Items.Any())
            {
                ViewBag.ErrorMessage = "Your cart is empty!";
                return View(cart);
            }

            // Populate ViewBag with cart details
            ViewBag.TotalItems = cart.TotalItems;
            ViewBag.TotalAmount = cart.TotalPrice;
            TempData["totalItems"] = cart.TotalItems;

            try
            {
                var itemQuantityMap = cart.Items.ToDictionary(i => i.ItemId, i => i.Quantity);

                Console.WriteLine("Calling ToPayment service with the following itemQuantityMap:");
                foreach (var item in itemQuantityMap)
                {
                    Console.WriteLine($"ItemId: {item.Key}, Quantity: {item.Value}");
                }

                var result = await _tableDetailService.ToPayment(itemQuantityMap, acc, CartSessionKey, HttpContext);

                Console.WriteLine($"ToPayment service returned: {result}");
                if (result)
                {
                    Console.WriteLine("Payment successful.");
                    ViewBag.SuccessMessage = "Payment processed successfully!";
                    cart.Items.Clear(); // Empty the cart after a successful payment
                    ViewBag.TotalItems = 0;
                    ViewBag.TotalAmount = 0;
                    // return View(cart);
                    return Ok("can get accesst to carrt");
                }
                else
                {
                    Console.WriteLine("Payment failed.");
                    ViewBag.ErrorMessage = "An error occurred during payment processing.";
                    // return View("Error");
                    return BadRequest("del on roi");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Cart: {ex.Message}");
                ViewBag.ErrorMessage = "An error occurred while processing your payment.";
                // return View("Error");
                return BadRequest("del on roiiiiiiiiiiiiiiiiii");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Cart: {ex.Message}");
            ViewBag.ErrorMessage = "An error occurred while loading the cart.";
            // return View("Error");
            return BadRequest("hetcuuuuuuuuuuuuuu");
        }
    }
    private CheckoutDTO GetCheckout()
    {
        // Retrieve cart from session
        var cart = HttpContext.Session.GetObjectFromJson<CheckoutDTO>(CartSessionKey);

        if (cart == null)
        {
            // Check for a cookie-based cart as a fallback
            if (Request.Cookies.TryGetValue(CartSessionKey, out var cookieCart))
            {
                cart = JsonSerializer.Deserialize<CheckoutDTO>(cookieCart) ?? new CheckoutDTO();
            }
            else
            {
                cart = new CheckoutDTO
                {
                    Items = new List<CheckoutItemDTO>() // Initialize empty list
                };
            }

            // Save cart back to session
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }

        return cart;
    }

    [HttpPost]
    public async Task<IActionResult> VNPayment(VnPaymentRequestModel paymentRequest)
    {
        // var username = HttpContext.Request.Cookies["username"];
        var username = User.Identity.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(new { message = "User is not logged in." });
        }

        // Lấy tài khoản từ username
        var acc = await _billRepository.GetAccountByUsername(username);
        if (acc == null)
        {
            return NotFound(new { message = "Account not found." });
        }

        // Truy xuất thông tin giỏ hàng dựa trên AccountId
        var cart = await _billRepository.GetBillAsync(acc.AccountId);
        if (cart == null)
        {
            return NotFound(new { message = "Cart not found." });
        }

        // Kiểm tra phương thức thanh toán
        if (paymentRequest.PaymentMethod != "Credit")
        {
            return BadRequest(new { message = "Invalid payment method." });
        }

        // Tạo mô hình yêu cầu thanh toán VNPAY
        var vnPayModel = new VnPaymentRequestModel
        {
            Amount = cart.TotalCost,
            CreatedDate = DateTime.Now,
            Description = $"{acc?.Username} {acc?.Username}", // Ví dụ thay bằng thông tin từ tài khoản
            PaymentMethod = paymentRequest.PaymentMethod
        };

        // Tạo URL thanh toán VNPAY
        var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);

        // Chuyển hướng người dùng đến trang thanh toán VNPAY
        return Redirect(paymentUrl);
    }



    public IActionResult AddToCart(int itemId)
    {
        var cart = GetCheckout();

        var item = _context.Items
                           .Where(i => i.ItemId == itemId)
                           .Select(i => new CheckoutItemDTO
                           {
                               ItemId = i.ItemId,
                               Name = i.ItemName,
                               Price = i.SuggestedPrice,
                               ImageUrl = i.ItemImage,
                               Quantity = 1
                           })
                           .FirstOrDefault();

        if (item == null)
        {
            return NotFound(new { message = "Item not found." });
        }

        // Add or update item quantity in cart
        var existingItem = cart.Items.FirstOrDefault(i => i.ItemId == item.ItemId);
        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            cart.Items.Add(item);
        }

        // Save cart to session
        HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

        // Update the total items in TempData
        TempData["totalItems"] = cart.Items.Sum(i => i.Quantity);

        // Redirect to Home page
        return RedirectToAction("Index", "Home");
    }

    public IActionResult RemoveFromCart(int itemId)
    {
        var cart = GetCheckout();
        var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item != null)
        {
            cart.Items.Remove(item);
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult UpdateCart(int itemId, int quantity)
    {
        var cart = GetCheckout();
        var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item != null && quantity > 0)
        {
            item.Quantity = quantity;
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        else if (quantity <= 0)
        {
            cart.Items.Remove(item);
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return RedirectToAction("Cart");
    }

    [Authorize]
    public IActionResult PaymentFail()
    {
        return View("PaymentFail");
    }

    [Authorize]
    public IActionResult PaymentSuccess()
    {
        return View("PaymentSuccess");
    }

    [Authorize]
    public IActionResult PaymentCallBack()
    {
        var response = _vnPayService.PaymentExecute(Request.Query);

        if (response == null || response.VnPayResponseCode != "00")
        {
            TempData["Message"] = $"Payment failed: {response?.VnPayResponseCode}";
            return RedirectToAction("PaymentFail");
        }

        TempData["Message"] = "Payment successful.";

        return RedirectToAction("PaymentSuccess");
    }
}