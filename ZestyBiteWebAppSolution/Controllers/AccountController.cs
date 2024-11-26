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
        private readonly TokenService _token;
        private readonly ApiClientService _apiClient;

        public AccountController(ApiClientService apiClientService, ILogger<AccountController> logger, IAccountService accountService, TokenService tokenService)
        {
            _logger = logger;
            _service = accountService;
            _token = tokenService;
            _apiClient = apiClientService;
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

        [HttpPost]
        [Route("LogIn")]
        public IActionResult LogIn([FromBody] LoginDTO dto)
        {
            // ModelState.AddModelError("UserName", "Tên đăng nhập không hợp lệ");

            if (_service.IsTrueAccount(dto.Username, dto.Password).Result)
            {
                string? roleDesc = _service.GetRoleDescByUsn(dto.Username).Result;
                if (roleDesc == null) return Unauthorized(new { Message = "RoleDesc is not found" });
                // Session stage
                HttpContext.Session.SetString("Username", dto.Username);
                HttpContext.Session.SetString("RoleDescription", roleDesc);

                List<string> roles = new List<string> { (roleDesc) };

                // string token = _token.GenerateToken(dto.Username, roleDesc.ToList()); 
                string token = _token.GenerateToken(dto.Username, roles); // worked by using new List<string>{}
                //  sol1
                // HttpContext.Response.Cookies.Append("Token", token);
                //  sol2
                /*  cách khác cụ thể hơn */
                HttpContext.Response.Cookies.Append("Token", token, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMinutes(2), // Thời gian hết hạn cookie
                    HttpOnly = true, // Giới hạn chỉ gửi cookie qua HTTP (không thể truy cập bằng JavaScript)
                    Secure = false, // Chỉ gửi cookie qua HTTPS (môi trường bảo mật), tạm thời để false để cho phép http test = vsc
                    SameSite = SameSiteMode.Strict // Chính sách SameSite giúp bảo vệ khỏi CSRF
                });

                return Ok(new { Token = token });

            }
            return Unauthorized();
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
        private LoginDTO AuthenticateUser(string usnomail, string pwd)
        {
            /*  logic from service maybe?*/
            return new LoginDTO { Email = usnomail, Username = usnomail, Password = pwd };
        }       // =-> không cần hàm này nữa vì đã có hàm called từ service "IsTrueAccount"
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Token");
            HttpContext.Session.Clear();
            RedirectToAction("Login", "Account");
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

                List<string> roles = new List<string> { created.RoleDescription };

                string token = _token.GenerateToken(created.Username, roles);
                // HttpContext.Response.Cookies.Append("Token", token);
                HttpContext.Response.Cookies.Append("Token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    Expires = DateTime.UtcNow.AddHours(1),
                    SameSite = SameSiteMode.Strict
                });
                Ok(new { Token = token });
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
            var userInfo = _token.GetUserInfoFromToken(token); // Lấy thông tin người dùng từ token

            if (userInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Trả về trang cá nhân với thông tin người dùng
            ViewBag.UserName = userInfo.UserName;
            ViewBag.Role = userInfo.Roles;

            // return Ok(userInfo);
            return View("AnotherViewName", userInfo);
            // anotherViewName = file cshtml .... AnotherViewName.cshtml
            // return View(); // Trả về trang cá nhân (Profile)
        }

        [HttpGet("profileee")] // => safer more security with RestfulAPI
        public IActionResult Profile()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userInfo = _token.GetUserInfoFromToken(token);

            if (userInfo == null)
            {
                return Unauthorized();
            }

            // Trả về thông tin người dùng
            return Ok(userInfo);
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

        [HttpGet("view/profile")]
        public async Task<IActionResult> ViewProfileee()
        {
            var token = HttpContext.Request.Cookies["Token"];  // Lấy token từ cookie

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "No token provided" });
            }

            var profileData = await _apiClient.GetProfileDataAsync(token);
            return Ok(new { ProfileData = profileData });
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

