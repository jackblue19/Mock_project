using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models;
using ZestyBiteWebAppSolution.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ZestyBiteWebAppSolution.Controllers {
    public class UserController : Controller {

        private readonly ZestyBiteContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly SignInManager<Accounts> _signInManager;
        private readonly UserManager<Accounts> _userManager;

        public UserController(ZestyBiteContext context, ILogger<UserController> logger, SignInManager<Accounts> signInManager, UserManager<Accounts> userManager) {
            _context = context;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult AccountPartial() {
            return PartialView();
        }

        [HttpGet]
        [Route("SignUp")]
        public IActionResult SignUp() {
            return View();
        }

        [HttpGet]
        [Route("SignIn")]
        public IActionResult SignIn() {
            return View();
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn(SignIn signIn) {
            if (ModelState.IsValid) {
                var account = await _context.Accounts.SingleOrDefaultAsync(a => a.UserName == signIn.Username);

                if (account != null) {
                    var hasher = new PasswordHasher<Accounts>();
                    var result = hasher.VerifyHashedPassword(account, account.PasswordHash, signIn.Password);

                    if (result == PasswordVerificationResult.Success) {
                        return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chính sau khi đăng nhập thành công
                    } else {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                } else {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(signIn);
        }



        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(SignUp signUp) {
            if (ModelState.IsValid) {
                bool gender = signUp.Gender == 1;

                // Tạo đối tượng Accounts từ SignUp
                Accounts accounts = new Accounts {
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    Email = signUp.Email,
                    UserName = signUp.Username,
                    RoleId = 7,
                    Gender = gender
                };

                // Băm mật khẩu
                var hasher = new PasswordHasher<Accounts>();
                accounts.PasswordHash = hasher.HashPassword(accounts, signUp.Password);

                try {
                    _context.Accounts.Add(accounts);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("SignIn", "User");
                } catch (Exception) {
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }
            return View(signUp);
        }

        public IActionResult EditProfile() {
            return View();
        }

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
    }
}
