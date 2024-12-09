using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using ZestyBiteWebAppSolution.Helpers;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Implementations;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers {
    [AllowAnonymous]
    public class AccountController : Controller {
        private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;
        private readonly IVerifyService _mailService;

        private static readonly
        ConcurrentDictionary<string, TaskCompletionSource<string>> VerificationTasks
                = new ConcurrentDictionary<string, TaskCompletionSource<string>>();
        private static readonly
        ConcurrentDictionary<string, int> VerificationAttempts
                = new ConcurrentDictionary<string, int>();

        public AccountController(IVerifyService verifyService, ILogger<AccountController> logger, IAccountService accountService) {
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
        public IActionResult ForgotPassword() {
            return View();
        }
        public IActionResult NewPassword() {
            return View();
        }


        [HttpPost]
        [Route("api/account/login")]
        //public async Task<IActionResult> Login(LoginDTO dto) {
        public async Task<IActionResult> Login([FromBody] LoginDTO dto) {

            if (!ModelState.IsValid) {
                return View(dto);
            }
            string lockoutKey = $"lockout_{dto.Username}";
            string attemptsKey = $"attempts_{dto.Username}";

            if (HttpContext.Session.TryGetValue(lockoutKey, out var lockoutEndBytes)) {
                var lockoutEnd = BitConverter.ToInt64(lockoutEndBytes, 0);
                if (lockoutEnd > DateTimeOffset.Now.ToUnixTimeSeconds()) {
                    var lockoutDateTime = DateTimeOffset.FromUnixTimeSeconds(lockoutEnd).ToLocalTime();
                    ModelState.AddModelError("", $"Account is locked until {lockoutDateTime:dd/MM/yyyy HH:mm:ss}");
                    return View(dto);
                } else {
                    HttpContext.Session.Remove(lockoutKey);
                }
            }

            int attempts = HttpContext.Session.GetInt32(attemptsKey) ?? 0;
            if (await _service.IsTrueAccount(dto.Username, dto.Password)) {
                try {
                    HttpContext.Session.SetString("username", dto.Username);
                    HttpContext.Session.Remove(attemptsKey); 

                    Response.Cookies.Append("username", dto.Username, new CookieOptions {
                        Expires = DateTimeOffset.Now.AddMinutes(30),
                        HttpOnly = true,
                        Secure = Request.IsHttps,
                        SameSite = SameSiteMode.Strict
                    });
                    var roleId = await _service.GetRoleIdByUsn(dto.Username);

                    if (roleId == 3) {
                        return RedirectToAction("Index", "Home", new { area = "Procurement_Manager" });
                    } else if (roleId == 1) {
                        return RedirectToAction("Index", "Home", new { area = "Manager" });
                    } else if (roleId == 4) {
                        return RedirectToAction("Index", "Home", new { area = "Server_Staff" });
                    } else if (roleId == 6) {
                        return RedirectToAction("Index", "Home", new { area = "Food_Runner" });
                    } else {
                        //return RedirectToAction("Index", "Home");
                        return Ok("OKKKKKKKKKKKKKK");
                    }
                } catch (Exception) {
                    ModelState.AddModelError("", "An unexpected error occurred during login.");
                    return View(dto);
                }
            } else {
                attempts++;
                HttpContext.Session.SetInt32(attemptsKey, attempts);
                if (attempts > 3) {
                    var lockoutEnd = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
                    HttpContext.Session.Set(lockoutKey, BitConverter.GetBytes(lockoutEnd));
                    HttpContext.Session.Remove(attemptsKey);
                    ModelState.AddModelError("", $"Account is locked until {DateTimeOffset.FromUnixTimeSeconds(lockoutEnd).ToLocalTime():dd/MM/yyyy HH:mm:ss}");
                    return View(dto);
                } else {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(dto);
                }
            }

        }

        [AllowAnonymous]
        [HttpPost]
        //[Route("api/account/newpwd")]
        public async Task<IActionResult> ForgotPassword( MailDTO dto)
        {
            string digit = VerificationCodeGenerator.GetSixDigitCode();
            var finding = await _service.GetAccByMailAsync(dto.Email);
            if (finding == null)
            {
                return BadRequest();
            }
            finding.VerificationCode = digit;
            await _service.UpdateVCode(finding);
            HttpContext.Session.SetString("username", finding.Username);
            Response.Cookies.Append("username", finding.Username, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(3),
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict
            });

            var pwdToken = new TaskCompletionSource<string>();
            var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));

            VerificationTasks[finding.Username] = pwdToken;
            VerificationAttempts[finding.Username] = 0;

            _ = Task.Delay(TimeSpan.FromMinutes(3), timeout.Token).ContinueWith(t =>
            {
                if (t.IsCanceled || t.IsFaulted)
                {
                    BadRequest("Program Crashing An EndPoint => out of time");
                }
                if (!pwdToken.Task.IsCompleted)
                {
                    VerificationTasks.TryRemove(finding.Username, out _);
                    VerificationAttempts.TryRemove(finding.Username, out _);
                    BadRequest("Program Crashing An EndPoint => out of time");
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            await _mailService.SendVerificationCodeAsync(dto.Email, digit);
            return RedirectToAction("NewPassword", "Account");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> NewPassword(ForgotPwdDTO dto)
        {

            var usn = User.Identity?.Name;
            if (string.IsNullOrEmpty(usn))
                return BadRequest("User not authenticated.");
            try
            {
                if (!VerificationTasks.ContainsKey(usn))
                {
                    return BadRequest("NO ACCOUNT WAS FOUND");
                }

                var pwdToken = VerificationTasks[usn];

                if (!VerificationAttempts.ContainsKey(usn))
                    VerificationAttempts[usn] = 0;

                if (VerificationAttempts[usn] >= 5)
                {
                    return BadRequest("Too many failed attempts.");
                }

                if (await _service.NewPwd(dto, usn))
                {
                    pwdToken.TrySetResult("Verified");
                    VerificationTasks.TryRemove(usn, out _);
                    VerificationAttempts.TryRemove(usn, out _);
                    HttpContext.Session.Remove("username");
                    Response.Cookies.Delete("username");
                    //return TypedResults.Ok("Change pwd sucess");
                    return RedirectToAction("Login", "Account");
                }

                VerificationAttempts[usn]++;
                if (VerificationAttempts[usn] >= 5)
                {
                    VerificationTasks.TryRemove(usn, out _);
                    VerificationAttempts.TryRemove(usn, out _);
                    // return RedirectToAction("Index", "Home");
                    return BadRequest();
                }
                return RedirectToAction("Index", "Home"); // trả về trang verify hiện tại
                //return TypedResults.BadRequest("Saii roi cung");
            } catch {
                return BadRequest(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyDTO verifyDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var usn = User.Identity?.Name;
            if (string.IsNullOrEmpty(usn))
                return BadRequest(new { Message = "User not authenticated." });

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
                    TempData["VerificationSuccess"] = "Email verification successful. Please log in.";
                    return RedirectToAction("Login", "Account");
                }

                VerificationAttempts[usn]++;

                if (VerificationAttempts[usn] >= 5) {
                    VerificationTasks.TryRemove(usn, out _);
                    VerificationAttempts.TryRemove(usn, out _);
                    if (await _service.IsDeleteUnregistedAccount(usn))
                        return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");
            } catch (Exception ex) {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO accountDto) {
            if (accountDto == null) return BadRequest(new { Message = "Invalid payload" });
            string token = VerificationCodeGenerator.GetSixDigitCode();
            accountDto.VerificationCode = token;
            try {
                var created = await _service.SignUpAsync(accountDto);
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

                _ = Task.Delay(TimeSpan.FromMinutes(3), cts.Token).ContinueWith(async t => {
                    if (t.IsCanceled || t.IsFaulted) {
                        await _service.IsDeleteUnregistedAccount(created.Username);
                        ViewBag.Error = "Verification failed! Out of time =DD";
                        return;
                    }

                    if (!tcs.Task.IsCompleted) {
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
            } catch (InvalidOperationException ex) {
                return BadRequest(new { Message = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500, new { Message = "Internal Server Error", Detail = ex.Message });
            }
        }

        [Route("viewprofile")]
        public async Task<IActionResult> ViewProfile() {
            try {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username)) {
                    return RedirectToAction("Login", "Account"); 
                }

                var dto = await _service.ViewProfileByUsnAsync(username);
                if (dto == null) {
                    return NotFound(); 
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
            } catch (Exception) {
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return RedirectToAction("ViewProfile");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePwdDTO dto) {
            if (!ModelState.IsValid) {
                return View(dto);
            }

            try {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username)) {
                    TempData["ErrorMessage"] = "You must be logged in to change your password.";
                    return RedirectToAction("Login", "Account");
                }

                // Kiểm tra mật khẩu xác nhận
                if (dto.NewPassword != dto.ConfirmNewPassword) {
                    TempData["ErrorMessage"] = "New password and confirmation password do not match.";
                    return View(dto);
                }

                var isOldPasswordCorrect = await _service.VerifyOldPasswordAsync(username, dto.OldPassword);
                if (!isOldPasswordCorrect) {
                    TempData["ErrorMessage"] = "Old password is incorrect.";
                    return View(dto);
                }

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