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
            return View(); // Hiển thị trang đăng nhập
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDTO dto) {
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDTO accountDto) {
            if (accountDto == null) return BadRequest(new { Message = "Invalid payload" });

            try {
                var created = await _service.SignUpAsync(accountDto);
                return RedirectToAction("Login", "Account");
            } catch (InvalidOperationException ex) {
                return BadRequest(new { Message = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500, new { Message = "Internal Server Error", Detail = ex.Message });
            }
        }

        [AllowAnonymous]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewProfile() {
            try {
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) {
                    return Unauthorized(); // Hoặc Redirect đến trang login
                }

                var dto = await _service.ViewProfileByUsnAsync(username);
                if (dto == null)
                    return NotFound(); // Trả về 404 nếu không tìm thấy tài khoản

                // Nếu bạn dùng MVC (thay vì API), trả về View và truyền model vào.
                return View(dto);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
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

        public IActionResult ChangePassword() {
            return View();
        }
        [AllowAnonymous]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePwdDTO dto) {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data received");

            try {
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) {
                    return Unauthorized(); // Hoặc Redirect đến trang login
                }

                await _service.ChangePwd(dto, username);
                return Ok(new { Message = "Password changed successfully" });
            } catch (Exception ex) {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO dto) {
            try {
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) return Unauthorized();
                await _service.UpdateProfile(dto, username);
                return RedirectToAction("ViewProfile", "Account");
            } catch (InvalidOperationException ex) {
                return BadRequest();
            }
        }

        public IActionResult VerifyEmail() {
            return View();
        }

        //[Route("Logout")]
        public IActionResult Logout() {
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("username");
            return RedirectToAction("Index", "Home");
        }
    }
}