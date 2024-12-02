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
    //[AllowAnonymous]
    [ApiController]
    [Route("api/[Controller]")]
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

        public IActionResult Login() {
            return View();
        }

        public IActionResult Register() {
            return View();
        }
        public IActionResult VerifyEmail() {
            return View();
        }
        public IActionResult ChangePassword() {
            return View();
        }

        [HttpPost]
        [Route("login")]
        //public async Task<IActionResult> Login([FromForm] LoginDTO dto)
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
            }

            return Unauthorized(new { message = "Invalid username or password" });
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
                if (string.IsNullOrEmpty(username)) {
                    return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                }

                var dto = await _service.ViewProfileByUsnAsync(username);
                if (dto == null) {
                    return NotFound(); // Hiển thị trang "Not Found" nếu không tìm thấy người dùng
                }

                return View(dto);
                // return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileDTO dto) {
            try {
                if (!ModelState.IsValid) {
                    TempData["ErrorMessage"] = "Invalid data. Please check your inputs.";
                    return RedirectToAction("ViewProfile");
                }

                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username)) {
                    TempData["ErrorMessage"] = "You must be logged in to update your profile.";
                    return RedirectToAction("LogIn", "Account");
                }

                await _service.UpdateProfile(dto, username); 

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("ViewProfile"); 
            } catch (InvalidOperationException ex) {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ViewProfile");
            } catch (Exception ex) {
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return RedirectToAction("ViewProfile");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePwdDTO dto) {
            if (!ModelState.IsValid) {
                TempData["ErrorMessage"] = "Invalid data received.";
                return View(dto); // Trả lại view và hiển thị lỗi
            }

            try {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username)) {
                    TempData["ErrorMessage"] = "You must be logged in to change your password.";
                    return RedirectToAction("Login", "Account"); // Nếu chưa đăng nhập, chuyển đến trang login
                }

                // Kiểm tra mật khẩu xác nhận
                if (dto.NewPassword != dto.ConfirmNewPassword) {
                    TempData["ErrorMessage"] = "New password and confirmation password do not match.";
                    return View(dto); 
                }

                // Kiểm tra mật khẩu cũ
                var isOldPasswordCorrect = await _service.VerifyOldPasswordAsync(username, dto.OldPassword);
                if (!isOldPasswordCorrect) {
                    TempData["ErrorMessage"] = "Old password is incorrect.";
                    return View(dto); 
                }

                // Cập nhật mật khẩu mới
                await _service.ChangePwd(dto, username);
                TempData["SuccessMessage"] = "Password changed successfully!";
                return View(dto); 

            } catch (Exception ex) {
                TempData["ErrorMessage"] = ex.Message;
                return View(dto); 
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
            return RedirectToAction("Index", "Home");
        }
    }
}