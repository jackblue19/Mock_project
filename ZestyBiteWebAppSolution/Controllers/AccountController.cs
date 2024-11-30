using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using ZestyBiteWebAppSolution.Helpers;
using ZestyBiteWebAppSolution.Services.Implementations;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ZestyBiteWebAppSolution.Controllers
{
    [AllowAnonymous]
    // [ApiController]
    // [Route("api/[Controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;
        private readonly IVerifyService _mailService;

        private static readonly
        ConcurrentDictionary<string, TaskCompletionSource<string>> VerificationTasks
                = new ConcurrentDictionary<string, TaskCompletionSource<string>>();
        private static readonly
        ConcurrentDictionary<string, int> VerificationAttempts
                = new ConcurrentDictionary<string, int>();

        public AccountController(IVerifyService verifyService, ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _service = accountService;
            _mailService = verifyService;
        }

        [HttpPost]
        // [Route("login")]
        // public async Task<IActionResult> Login([FromForm] LoginDTO dto)
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

                return RedirectToAction("Index", "Home");
                // return Ok("Log in suceess");
            }
            //  báo lỗi login vì sao fail

            return Unauthorized(new { message = "Invalid username or password" });
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        // [Route("VerifyCode")]
        // public async Task<IActionResult> VerifyCode([FromForm] VerifyDTO verifyDto)
        public async Task<IActionResult> VerifyEmail( /*[FromBody]*/ VerifyDTO verifyDto)
        {
            var usn = User.Identity.Name;

            if (verifyDto == null || string.IsNullOrEmpty(usn) || string.IsNullOrEmpty(verifyDto.Code))
                return BadRequest(new { Message = "Invalid verification data" });

            try
            {
                // if (!VerificationTasks.TryGetValue(usn, out var tcs))
                //     return BadRequest(new { Message = "Verification session expired or not found." });
                if (!VerificationTasks.ContainsKey(usn))
                {
                    await _service.IsDeleteUnregistedAccount(usn);
                    return BadRequest(new { Message = "Verification session expired or not found." });
                }
                var tcs = VerificationTasks[usn];

                if (!VerificationAttempts.ContainsKey(usn))
                    VerificationAttempts[usn] = 0;

                if (VerificationAttempts[usn] >= 5)
                {
                    await _service.IsDeleteUnregistedAccount(usn);
                    return BadRequest(new { Message = "Too many failed attempts." });
                }

                if (await _service.IsVerified(usn, verifyDto.Code))
                {
                    tcs.TrySetResult("Verified");
                    VerificationTasks.TryRemove(usn, out _);
                    VerificationAttempts.TryRemove(usn, out _);
                    HttpContext.Session.Remove("username");
                    Response.Cookies.Delete("username");
                    // return Ok(new { Message = "Verification successful." });
                    return RedirectToAction("Login", "Account");
                }

                VerificationAttempts[usn]++;
                if (VerificationAttempts[usn] >= 5)
                {
                    VerificationTasks.TryRemove(usn, out _);
                    VerificationAttempts.TryRemove(usn, out _);
                    if (await _service.IsDeleteUnregistedAccount(usn))
                        // return BadRequest(new { Message = "Account deleted due to too many failed attempts." });
                        return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
                // return BadRequest(new { Message = "Invalid verification code. Try again." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost]
        // [Route("signup")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO accountDto)
        // public async Task<IActionResult> Register([FromBody] RegisterDTO accountDto)
        {
            if (accountDto == null) return BadRequest(new { Message = "Invalid payload" });
            string token = VerificationCodeGenerator.GetSixDigitCode();
            accountDto.VerificationCode = token;
            try
            {
                var created = await _service.SignUpAsync(accountDto);
                // return RedirectToAction("Login", "Account");
                HttpContext.Session.SetString("username", created.Username);
                Response.Cookies.Append("username", created.Username, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddMinutes(3),
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict
                });

                // Tạo CancellationTokenSource và TaskCompletionSource
                var tcs = new TaskCompletionSource<string>();
                var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3)); // gioi han thgian = 3 min

                VerificationTasks[created.Username] = tcs;
                VerificationAttempts[created.Username] = 0;

                // không dùng await vì muốn thực hiện song song với hàm VerifyCode
                _ = Task.Delay(TimeSpan.FromMinutes(3), cts.Token).ContinueWith(async t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        await _service.IsDeleteUnregistedAccount(created.Username);
                        ViewBag.Error = "Verification failed! Out of time =DD";
                        return;
                    }

                    if (!tcs.Task.IsCompleted)
                    {
                        VerificationTasks.TryRemove(created.Username, out _);
                        VerificationAttempts.TryRemove(created.Username, out _);
                        if (await _service.IsDeleteUnregistedAccount(created.Username))
                            ViewBag.Error = "Verification failed! Out of time =DD";
                        else
                            ViewBag.Error = "Created?";
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
                await _mailService.SendVerificationCodeAsync(accountDto.Email, token);
                return RedirectToAction("VerifyEmail", "Account");
                // return Ok(token);
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


        [HttpGet]
        [Route("viewprofile")]
        public async Task<IActionResult> ViewProfile()
        {
            try
            {
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(); // Hoặc Redirect đến trang login
                }

                var dto = await _service.ViewProfileByUsnAsync(username);
                if (dto == null)
                    return NotFound();

                // return View(dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("updateprofile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { Message = "Profile data is missing." });
                }

                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) return Unauthorized();

                // Gọi service để cập nhật thông tin
                await _service.UpdateProfile(dto, username);

                // Trả về kết quả thành công
                return Ok(new { Message = "Profile updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [HttpPut]
        [Route("changepwd")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePwdDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data received");

            try
            {
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(); // Hoặc Redirect đến trang login
                }

                await _service.ChangePwd(dto, username);
                return Ok(new { Message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("getallacc")]
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

        [HttpPost]
        // [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("username");
            // return RedirectToAction("Index", "Home");
            return Ok("Log out done");
        }
    }
}