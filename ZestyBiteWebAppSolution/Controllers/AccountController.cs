using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using ZestyBiteWebAppSolution.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Humanizer;
using NuGet.Common;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Authorization;

namespace ZestyBiteWebAppSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _service = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccountPartial()
        {
            return PartialView();
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /* Login */
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Hiển thị trang đăng nhập
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginDTO dto)
        {
            // ModelState.AddModelError("UserName", "Tên đăng nhập không hợp lệ");

            if (await _service.IsTrueAccount(dto.Username, dto.Password))
            {
                HttpContext.Session.SetString("username", dto.Username);
                Response.Cookies.Append("username", dto.Username, new CookieOptions
                {
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
            // return View();
            /*
            if (ModelState.IsValid)
        {
            // Giả sử bạn đã kiểm tra thông tin người dùng hợp lệ (ví dụ, từ cơ sở dữ liệu)
            var user = AuthenticateUser(model.UserName, model.Password);  // Giả sử phương thức này kiểm tra người dùng

            if (user != null)
            {
                // Sau khi xác thực thành công, lưu thông tin vào session
                HttpContext.Session.SetString("UserName", user.UserName);  // Lưu tên người dùng vào session
                HttpContext.Session.SetString("UserRole", user.Role);      // Lưu vai trò của người dùng vào session

                return RedirectToAction("Index", "Home");  // Chuyển hướng tới trang chủ sau khi đăng nhập
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
            }
        }

        return View(model);  // Trả lại trang đăng nhập nếu đăng nhập thất bại
            */
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("username");
            // RedirectToAction("Login", "Account");
            return Ok("Log out sucessfully");
            // thêm case tắt trình duyệt thì auto log out
        }
        public IActionResult Logout2()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //[Route("LogIn")]
        //public async Task<IActionResult> LogIn(LogIn logIn) {
        //    return View(logIn);
        //}

        //GET: api/Account
        [AllowAnonymous]  // bỏ qua middleware => ko cần xác thực authN
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AccountDTO accountDto)
        {
            if (accountDto == null) return BadRequest(new { Message = "Invalid payload" });

            try
            {
                var created = await _service.SignUpAsync(accountDto);
                return Created($"/api/account/{created.Id}", created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Detail = ex.Message });
            }
        }

        [HttpPost("signup2")]
        public async Task<IResult> SignUpSecond([FromBody] AccountDTO acc)
        {
            try
            {
                var created = await _service.SignUpAsync(acc);
                // return RedirectToAction("Profile", "User");return RedirectToAction("Profile", "User");
                return TypedResults.Created($"/api/account/{created.Id}", created);
            }
            catch (InvalidOperationException ex)
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]     // => buộc thông qua middleware -> authN
        [HttpGet("profile/{id}")]
        public async Task<IResult> ViewProfile(int id)
        {
            try
            {
                var dto = await _service.GetAccountByIdAsync(id);
                if (dto == null)
                    return TypedResults.NotFound();
                // return TypedResults.Ok($"/api/account/{account.UserName}", account); // => usage for page redirect
                return TypedResults.Ok(dto); // => api in json only =)))

            }
            catch (InvalidOperationException ex)
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
            catch (ArgumentException ex) // Xử lý lỗi từ service
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex) // Bắt lỗi bất kỳ khác
            {
                return TypedResults.Problem($"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult ProfileViewing()
        {
            var token = HttpContext.Request.Cookies["Token"]; // Lấy JWT token từ cookie

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account"); // Nếu không có token, yêu cầu đăng nhập
            }

            // Thực hiện xác thực token và lấy thông tin người dùng từ token
            var userInfo = "GetUserInfoFromToken(token)"; // Lấy thông tin người dùng từ token

            if (userInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Trả về trang cá nhân với thông tin người dùng
            ViewBag.UserName = "userInfo.UserName";
            ViewBag.Role = "userInfo.Roles";

            // return Ok(userInfo);
            return View("AnotherViewName", userInfo);
            // anotherViewName = file cshtml .... AnotherViewName.cshtml
            // return View(); // Trả về trang cá nhân (Profile)
        }


        [Authorize]
        [HttpGet("profile/only/{username}")]
        public async Task<IActionResult> ViewProfile(string username)
        {
            try
            {
                var dto = await _service.GetAccountByUsnAsync(username);
                if (dto == null)
                    return NotFound(); // trả về 404 nếu không tìm thấy

                // Sau khi lấy được dto, chuyển hướng đến một trang MVC khác (Profile view)
                return RedirectToAction("Profile", "User", new { username = dto.Username }); // Chuyển hướng đến action Profile của UserController
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message }); // trả về lỗi nếu có
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem($"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IResult> GetAllAccount()
        {
            try
            {
                var accounts = await _service.GetALlAccountAsync();
                if (!accounts.Any()) return TypedResults.NotFound();
                return TypedResults.Ok(accounts);

            }
            catch (InvalidOperationException ex)
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("changepassword")]
        public async Task<IResult> ChangePassword([FromBody] ChangePwdDTO dto)
        {
            try
            {
                await _service.ChangePwd(dto);
                return TypedResults.Ok(dto);    // dùng tạm thời để check ngay trên response
                // return TypedResults.NoContent(); //  => nếu hoành thành rồi thì nên trả về NoContent cho chuẩn
            }
            catch (InvalidOperationException ex)
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("modify")]

        public async Task<IResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
        {
            try
            {
                await _service.UpdateProfile(dto);
                return TypedResults.Ok(dto);    // dùng tạm thời để check ngay trên response
                // return TypedResults.NoContent(); //  => nếu hoành thành rồi thì nên trả về NoContent cho chuẩn
            }
            catch (InvalidOperationException ex)
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        public IActionResult EditProfile()
        {
            return View();
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }



    }
}

