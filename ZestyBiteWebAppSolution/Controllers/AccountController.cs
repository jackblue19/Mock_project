using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;

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

        [HttpGet]
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            return View();
        }

        //[HttpPost]
        //[Route("LogIn")]
        //public async Task<IActionResult> LogIn(LogIn logIn) {
        //    return View(logIn);
        //}

        //GET: api/Account
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
                return TypedResults.Created($"/api/account/{created.Id}", created);
            }
            catch (InvalidOperationException ex)
            {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }
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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
