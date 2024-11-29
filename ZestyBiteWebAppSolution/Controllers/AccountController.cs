using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers {
    [AllowAnonymous]
    public class AccountController : Controller {
        private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService) {
            _logger = logger;
            _service = accountService;
        }

        public IActionResult Login() {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (await _service.IsTrueAccount(dto.Username, dto.Password)) {
                HttpContext.Session.SetString("username", dto.Username);
                Response.Cookies.Append("username", dto.Username, new CookieOptions {
                    Expires = DateTimeOffset.Now.AddMinutes(30),
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict
                });

                return RedirectToAction("Index", "Home");
            }

            return Unauthorized(new { message = "Invalid username or password" });
        }
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO accountDto) {
            if (accountDto == null) {
                ModelState.AddModelError(string.Empty, "Invalid payload.");
                return View(accountDto); // Trả về view cùng dữ liệu người dùng đã nhập.
            }

            if (!ModelState.IsValid) {
                // Nếu dữ liệu không hợp lệ, trả về view cùng thông báo lỗi
                return View(accountDto);
            }

            try {
                var created = await _service.SignUpAsync(accountDto);

                if (created != null) {
                    TempData["SuccessMessage"] = "Account created successfully. Please log in.";
                    return RedirectToAction("Login", "Account");
                }

                // Nếu không tạo được tài khoản, thêm lỗi và trả về view
                ModelState.AddModelError(string.Empty, "Failed to create the account. Please try again.");
                return View(accountDto);
            } catch (InvalidOperationException ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(accountDto);
            } catch (Exception) {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(accountDto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> ViewProfile() {
            try {
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) {
                    return Unauthorized(); // Hoặc Redirect đến trang login
                }

                var dto = await _service.ViewProfileByUsnAsync(username);
                if (dto == null)
                    return NotFound();

                return View(dto);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileDTO dto) {
            try {
                if (!ModelState.IsValid) {
                    return View("ViewProfile", dto);
                }

                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) return Unauthorized();

                await _service.UpdateProfile(dto, username);

                ViewBag.SuccessMessage = "Profile updated successfully."; 
                return View("ViewProfile", dto); 
            } catch (InvalidOperationException ex) {
                ViewBag.ErrorMessage = ex.Message;
                return View("ViewProfile", dto);
            } catch (Exception ex) {
                ViewBag.ErrorMessage = "Internal Server Error: " + ex.Message;
                return View("ViewProfile", dto);
            }
        }

        [HttpGet]
        public IActionResult ChangePassword() { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePwdDTO dto) {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data received");

            try {
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) {
                    return Unauthorized();
                }

                // Gọi service để thay đổi mật khẩu
                await _service.ChangePwd(dto, username);

                // Thông báo thành công
                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToAction("ViewProfile", "Account");  
            } catch (Exception) {
                // Xử lý lỗi nếu có
                TempData["ErrorMessage"] = "An error occurred while changing password!";
                return View(dto);
            }
        }


        [Authorize(Roles = "Manager")]
        public async Task<IResult> GetAllAccount() {
            try {
                var accounts = await _service.GetALlAccountAsync();
                if (!accounts.Any()) return TypedResults.NotFound();
                return TypedResults.Ok(accounts);

            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        public IActionResult Logout() {
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("username");
            return RedirectToAction("Index", "Home");
        }
    }
}