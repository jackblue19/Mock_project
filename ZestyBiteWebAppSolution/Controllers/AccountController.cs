using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using ZestyBiteWebAppSolution.Helpers;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers
{
    [AllowAnonymous]
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

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IVerifyService verifyService)
        {
            _logger = logger;
            _service = accountService;
            _mailService = verifyService;
        }

        public IActionResult VerifyEmail() {
            return View();
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDTO verifyDto) {
            var usn = User.Identity.Name;

            if (verifyDto == null || string.IsNullOrEmpty(usn) || string.IsNullOrEmpty(verifyDto.Code))
                return BadRequest(new { Message = "Invalid verification data" });

            try {
                if (!VerificationTasks.ContainsKey(usn)) {
                    await _service.IsDeleteUnregistedAccount(usn);
                    return BadRequest(new { Message = "Verification session expired or not found." });
                }

                var tcs = VerificationTasks[usn];

                if (!VerificationAttempts.ContainsKey(usn))
                    VerificationAttempts[usn] = 0;

                if (VerificationAttempts[usn] >= 5) {
                    await _service.IsDeleteUnregistedAccount(usn);
                    return BadRequest(new { Message = "Too many failed attempts." });
                }

                if (await _service.IsVerified(usn, verifyDto.Code)) {
                    tcs.TrySetResult("Verified");
                    VerificationTasks.TryRemove(usn, out _);
                    VerificationAttempts.TryRemove(usn, out _);
                    HttpContext.Session.Remove("username");
                    Response.Cookies.Delete("username");

                    return RedirectToAction("Login", "Account");
                }

                VerificationAttempts[usn]++;
                if (VerificationAttempts[usn] >= 5) {
                    VerificationTasks.TryRemove(usn, out _);
                    VerificationAttempts.TryRemove(usn, out _);
                    if (await _service.IsDeleteUnregistedAccount(usn))
                        return BadRequest(new { Message = "Account deleted due to too many failed attempts." });
                }

                return BadRequest(new { Message = "Invalid verification code. Try again." });
            } catch (Exception ex) {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }


        public IActionResult Login() {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto) {
            if (!ModelState.IsValid) {
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO accountDto) {
            if (accountDto == null) {
                ModelState.AddModelError(string.Empty, "Invalid payload.");
                return View(accountDto);
            }

            if (!ModelState.IsValid) {
                return View(accountDto);  
            }

            string token = VerificationCodeGenerator.GetSixDigitCode();
            accountDto.VerificationCode = token;

            try {
                var created = await _service.SignUpAsync(accountDto);

                if (created != null) {
                    HttpContext.Session.SetString("username", created.Username);
                    Response.Cookies.Append("username", created.Username, new CookieOptions {
                        Expires = DateTimeOffset.Now.AddMinutes(3),
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Strict
                    });

                    var tcs = new TaskCompletionSource<string>();
                    var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));
                    VerificationTasks[created.Username] = tcs;
                    VerificationAttempts[created.Username] = 0;

                    _ = Task.Delay(TimeSpan.FromMinutes(3), cts.Token).ContinueWith(async t =>
                    {
                        if (t.IsCanceled || t.IsFaulted) {
                            await _service.IsDeleteUnregistedAccount(created.Username);
                            TempData["Error"] = "Verification failed! Out of time.";
                            return;
                        }

                        if (!tcs.Task.IsCompleted) {
                            VerificationTasks.TryRemove(created.Username, out _);
                            VerificationAttempts.TryRemove(created.Username, out _);
                            if (await _service.IsDeleteUnregistedAccount(created.Username))
                                TempData["Error"] = "Verification failed! Out of time.";
                            else
                                TempData["Error"] = "Account not verified.";
                        }
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);

                    await _mailService.SendVerificationCodeAsync(accountDto.Email, token);

                    TempData["SuccessMessage"] = "Account created successfully. Please check your email for the verification code.";
                    return RedirectToAction("VerifyEmail", "Account");
                }

                ModelState.AddModelError(string.Empty, "Failed to create the account. Please try again.");
                return View(accountDto);
            } catch (InvalidOperationException ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(accountDto);
            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                TempData["Error"] = $"Error Details: {ex.Message}";  
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

                // Lấy thông tin từ dịch vụ
                var dto = await _service.ViewProfileByUsnAsync(username);

                if (dto == null) {
                    return NotFound("Profile not found.");
                }

                return View(dto);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileDTO dto) {
            try {

                if (!ModelState.IsValid) {
                    return View("ViewProfile", dto); // Hiển thị lại form với lỗi xác thực
                }

                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username)) {
                    return Unauthorized();
                }

                // Gọi service để cập nhật dữ liệu
                await _service.UpdateProfile(dto, username);

                // Use TempData to pass the success message after redirect
                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction("ViewProfile");
            } catch (Exception ex) {
                // Use TempData to pass the error message after redirect
                TempData["ErrorMessage"] = $"Internal Server Error: {ex.Message}";
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

            try
            {
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
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("username");
            return RedirectToAction("Index", "Home");
        }
    }
}