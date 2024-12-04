using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models;
using ZestyBiteWebAppSolution.Models.ViewModel;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

public class CartController : Controller
{
    private const string CartSessionKey = "Cart";
    private readonly ZestyBiteContext _context;
    private readonly IVnPayService _vnPayService;
    private readonly IBillRepository _billRepository;

    public CartController(ZestyBiteContext context, IVnPayService vnPayService, IBillRepository billRepository)
    {
        _context = context;
        _vnPayService = vnPayService;
        _billRepository = billRepository;
    }

    // Display Cart
    public IActionResult Cart()
    {
        try
        {
            var cart = GetCheckout(); // Retrieve cart from session
            if (cart == null || !cart.Items.Any())
            {
                ViewBag.ErrorMessage = "Your cart is empty!";
                return View(cart); // Display empty cart message
            }

            ViewBag.TotalItems = cart.Items.Sum(i => i.Quantity);
            ViewBag.TotalAmount = cart.Items.Sum(i => i.Quantity * i.Price);
            return View(cart); // Automatically maps to Views/Cart/Cart.cshtml
        }
        catch (Exception)
        {
            // Log the error (logging not shown here)
            return View("Error", new { message = "Failed to load the cart." });
        }
    }
    public IActionResult Checkout()
    {
        var cart = GetCheckout(); // Assume GetCheckout() retrieves cart from session
        if (cart == null || !cart.Items.Any())
        {
            return RedirectToAction("Cart"); // Redirect to cart if it's empty
        }

        var checkoutDTO = new CheckoutDTO
        {
            Items = cart.Items,
            TotalAmount = cart.Items.Sum(i => i.Quantity * i.Price)
        };

        return View("Checkout", checkoutDTO);
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

    private CheckoutDTO GetCheckout()
    {
        var cart = HttpContext.Session.GetObjectFromJson<CheckoutDTO>(CartSessionKey);
        if (cart == null)
        {
            cart = new CheckoutDTO
            {
                Items = new List<CheckoutItemDTO>() // Initialize empty list
            };
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return cart;
    }

    public IActionResult AddToCart(int itemId)
    {

        var cart = GetCheckout();
        // Fetch item from database
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

        // Save session
        HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        var totalItems = cart.Items.Sum(i => i.Quantity);

        return RedirectToAction("Index", "Home", new { cartTotalItems = totalItems });
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