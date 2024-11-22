using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers {
    public class AccountController : Controller {
        private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailService _emailService;
        private readonly IEmailConfirmationRepository _emailConfirmationRepository;
        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IEmailService emailService, IEmailConfirmationRepository emailConfirmationRepository) {
            _logger = logger;
            _service = accountService;
            _emailService = emailService;
            _emailConfirmationRepository = emailConfirmationRepository;
        }

        private async Task SendEmailAsync(string email, string subject, string message) {
            await _emailService.SendEmailAsync(email, subject, message);
        }

        private string GenerateEmailConfirmationToken() {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult AccountPartial() {
            return PartialView();
        }

        private string HashPassword(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        [HttpGet]
        [Route("LogIn")]
        public IActionResult LogIn() {
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
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AccountDTO accountDto) {
            if (accountDto == null)
                return BadRequest(new { Message = "Invalid payload" });

            try {
                // Tạo tài khoản
                var created = await _service.SignUpAsync(accountDto);

                // Sinh mã xác nhận email
                var token = GenerateEmailConfirmationToken();

                // Lưu mã xác nhận vào repository
                await _emailConfirmationRepository.SaveConfirmationTokenAsync(accountDto.Email, token);

                // Tạo link xác nhận email
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { email = accountDto.Email, token },
                    Request.Scheme);

                // Gửi email xác nhận
                var subject = "Email Confirmation - Complete Your Registration";
                var body = $"Hi {accountDto.Username},<br><br>" +
                           $"Please confirm your email by clicking the link below:<br>" +
                           $"<a href='{confirmationLink}'>Confirm Email</a>";

                await _emailService.SendEmailAsync(accountDto.Email, subject, body);

                return Created($"/api/account/{created.Id}", new {
                    created.Id,
                    created.Email,
                    Message = "Registration successful. Please check your email to confirm your account."
                });
            } catch (InvalidOperationException ex) {
                return BadRequest(new { Message = ex.Message });
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occurred during registration.");
                return StatusCode(500, new { Message = "Internal Server Error", Detail = ex.Message });
            }

        }

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token) {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest(new { Message = "Invalid email or token" });

            try {
                // Kiểm tra token
                var isValidToken = ValidateEmailConfirmationToken(token, email);
                if (!isValidToken)
                    return BadRequest(new { Message = "Invalid or expired confirmation token" });

                // Xác nhận email mà không cần lưu kết quả vào biến
                await _service.ConfirmEmailAsync(email);

                return Ok(new { Message = "Email confirmed successfully!" });
            } catch (Exception ex) {
                return StatusCode(500, new { Message = "Internal Server Error", Detail = ex.Message });
            }
        }


        [HttpGet("profile/{id}")]
        public async Task<IResult> ViewProfile(int id) {
            try {
                var dto = await _service.GetAccountByIdAsync(id);
                if (dto == null)
                    return TypedResults.NotFound();
                // return TypedResults.Ok($"/api/account/{account.UserName}", account); // => usage for page redirect
                return TypedResults.Ok(dto); // => api in json only =)))

            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            } catch (ArgumentException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            } catch (Exception ex) {
                return TypedResults.Problem($"Internal Server Error: {ex.Message}");
            }

        }
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

        [HttpPut("changepassword")]
        public async Task<IResult> ChangePassword([FromBody] ChangePwdDTO dto) {
            try {
                await _service.ChangePwd(dto);
                return TypedResults.Ok(dto);    // dùng tạm thời để check ngay trên response
                // return TypedResults.NoContent(); //  => nếu hoành thành rồi thì nên trả về NoContent cho chuẩn
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("modify")]
        public async Task<IResult> UpdateProfile([FromBody] UpdateProfileDTO dto) {
            try {
                await _service.UpdateProfile(dto);
                return TypedResults.Ok(dto);    // dùng tạm thời để check ngay trên response
                // return TypedResults.NoContent(); //  => nếu hoành thành rồi thì nên trả về NoContent cho chuẩn
            } catch (InvalidOperationException ex) {
                return TypedResults.BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("VerifyEmail")]
        public IActionResult VerifyEmail() {
            return View();
        }

        public IActionResult ChangePassword() {
            return View();
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        // Phương thức xác thực token
        private bool ValidateEmailConfirmationToken(string token, string email) {
            try {
                // Giải mã token từ base64
                var tokenData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = tokenData.Split(':');

                if (parts.Length != 2) {
                    return false; // Nếu token không hợp lệ
                }

                var tokenEmail = parts[0];
                var tokenTime = DateTime.Parse(parts[1]);

                // Kiểm tra email trong token
                if (tokenEmail != email) {
                    return false; // Email không khớp
                }

                // Kiểm tra thời gian hết hạn (ví dụ: token hết hạn sau 24 giờ)
                if (DateTime.UtcNow - tokenTime > TimeSpan.FromHours(24)) {
                    return false; // Token hết hạn
                }

                return true; // Token hợp lệ
            } catch (Exception) {
                return false; // Lỗi khi giải mã hoặc định dạng không hợp lệ
            }
        }
    }
}
