using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ZestyBiteWebAppSolution.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller {
        private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService) {
            _logger = logger;
            _service = accountService;
        }

        /* Login */
        [HttpGet]
        public IActionResult Login() {

            return View(); 
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginDTO dto) {
            // ModelState.AddModelError("UserName", "Tên đăng nhập không hợp lệ");
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("username");
            if (await _service.IsTrueAccount(dto.Username, dto.Password)) {
                HttpContext.Session.SetString("username", dto.Username);
                Response.Cookies.Append("username", dto.Username, new CookieOptions {
                    Expires = DateTimeOffset.Now.AddMinutes(30),
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict
                });
                return Ok("Login sucessfully");
                // return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid username or password";
            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult Logout() {
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("username");
            // RedirectToAction("Login", "Account");
            return Ok("Log out sucessfully");
            //  return RedirectToAction("Index", "Home");
            //  thêm case tắt trình duyệt thì auto log out
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Register")]
        public IActionResult Register() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IResult> Register([FromBody] RegisterDTO acc) {
            try {
                var created = await _service.SignUpAsync(acc);
                // return RedirectToAction("Profile", "User");return RedirectToAction("Profile", "User");
                return TypedResults.Created($"/api/account/{created.Id}", created);
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IResult> ViewProfile() {
            try {
                var username = User.Identity.Name;

                if (string.IsNullOrEmpty(username)) {
                    // return RedirectToAction("Login", "Account");
                    return (IResult)Unauthorized();
                }
                var dto = await _service.GetAccountByUsnAsync(username);

                if (dto == null)
                    return TypedResults.NotFound();
                // return TypedResults.Ok($"/api/account/{account.UserName}", account); // => usage for page redirect
                return TypedResults.Ok(dto); // => api in json only =)))
                // return RedirectToAction("Profile", "User", new { username = dto.Username }); // Chuyển hướng đến action Profile của UserController
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            } catch (ArgumentException ex) // Xử lý lỗi từ service
              {
                return TypedResults.BadRequest(new { Message = ex.Message });
            } catch (Exception ex) // Bắt lỗi bất kỳ khác
              {
                return TypedResults.Problem($"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("all")]
        public async Task<IResult> GetAllAccount() {
            try {
                var accounts = await _service.GetALlAccountAsync();
                if (!accounts.Any()) return TypedResults.NotFound();
                return TypedResults.Ok(accounts);

            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("changepassword")]
        public async Task<IResult> ChangePassword([FromBody] ChangePwdDTO dto) {
            if (ModelState.IsValid) {
                var username = User.Identity.Name;

                if (string.IsNullOrEmpty(username))
                    // return RedirectToAction("Login", "Account");
                    return (IResult)Unauthorized();
                try {
                    await _service.ChangePwd(dto, username);
                    return TypedResults.Ok(dto);    // dùng tạm thời để check ngay trên response
                                                    // return TypedResults.NoContent(); //  => nếu hoành thành rồi thì nên trả về NoContent cho chuẩn
                } catch (InvalidOperationException ex) {
                    return TypedResults.BadRequest(new { Message = ex.Message });
                }
            }
            return TypedResults.BadRequest();
        }

        [Authorize]
        [HttpPut("modify")]
        public async Task<IResult> UpdateProfile([FromBody] UpdateProfileDTO dto) {
            try {
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) return (IResult)Unauthorized();
                await _service.UpdateProfile(dto, username);
                return TypedResults.Ok(dto);    // dùng tạm thời để check ngay trên response
                // return TypedResults.NoContent(); //  => nếu hoành thành rồi thì nên trả về NoContent cho chuẩn
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        public IActionResult VerifyEmail() {
            return View();
        }
    }
}