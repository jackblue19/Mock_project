using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using ZestyBiteWebAppSolution.Helpers;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Implementations;
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

        public AccountController(IVerifyService verifyService, ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _service = accountService;
            _mailService = verifyService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
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
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult NewPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if (await _service.IsTrueAccount(dto.Username, dto.Password))
            {
                try
                {
                    HttpContext.Session.SetString("username", dto.Username);
                    Response.Cookies.Append("username", dto.Username, new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddMinutes(30),
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Strict
                    });
                    // return Ok("Login done");
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    throw new Exception("dunno error");
                }
            }

            return Unauthorized(new { message = "Invalid username or password" });
        }

        // [HttpPost]
        // public async Task<IActionResult> ForgotPassword(string email)
        // {
        //     string verificationCode = VerificationCodeGenerator.GetSixDigitCode();
        //     HttpContext.Session.SetString("username", email);
        // }

        // [HttpPost]
        // public async Task<IActionResult> NewPassword([FromBody] ForgotPwdDTO dto)
        // {

        // }


        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyDTO verifyDto)
        {
            var usn = User.Identity?.Name;

            if (verifyDto == null || string.IsNullOrEmpty(usn) || string.IsNullOrEmpty(verifyDto.Code))
                return BadRequest(new { Message = "Invalid verification data" });

            try
            {
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
                    return RedirectToAction("Login", "Account");
                }

                VerificationAttempts[usn]++;
                if (VerificationAttempts[usn] >= 5)
                {
                    VerificationTasks.TryRemove(usn, out _);
                    VerificationAttempts.TryRemove(usn, out _);
                    if (await _service.IsDeleteUnregistedAccount(usn))
                        return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO accountDto)
        {
            if (accountDto == null) return BadRequest(new { Message = "Invalid payload" });
            string token = VerificationCodeGenerator.GetSixDigitCode();
            accountDto.VerificationCode = token;
            try
            {
                var created = await _service.SignUpAsync(accountDto);
                HttpContext.Session.SetString("username", created.Username);
                Response.Cookies.Append("username", created.Username, new CookieOptions
                {
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
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
                }

                var dto = await _service.ViewProfileByUsnAsync(username);
                if (dto == null)
                {
                    return NotFound(); // Hiển thị trang "Not Found" nếu không tìm thấy người dùng
                }
                return View(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid data. Please check your inputs.";
                    return RedirectToAction("ViewProfile");
                }

                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    TempData["ErrorMessage"] = "You must be logged in to update your profile.";
                    return RedirectToAction("LogIn", "Account");
                }

                await _service.UpdateProfile(dto, username);

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("ViewProfile");
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ViewProfile");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return RedirectToAction("ViewProfile");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePwdDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data received.";
                return View(dto);
            }

            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    TempData["ErrorMessage"] = "You must be logged in to change your password.";
                    return RedirectToAction("Login", "Account");
                }

                // Kiểm tra mật khẩu xác nhận
                if (dto.NewPassword != dto.ConfirmNewPassword)
                {
                    TempData["ErrorMessage"] = "New password and confirmation password do not match.";
                    return View(dto);
                }

                var isOldPasswordCorrect = await _service.VerifyOldPasswordAsync(username, dto.OldPassword);
                if (!isOldPasswordCorrect)
                {
                    TempData["ErrorMessage"] = "Old password is incorrect.";
                    return View(dto);
                }

                await _service.ChangePwd(dto, username);
                TempData["SuccessMessage"] = "Password changed successfully!";
                return View(dto);

            }
            catch (Exception ex)
            {
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
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            Response.Cookies.Delete("username");
            return RedirectToAction("Index", "Home");
        }
    }
}