using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models1.Entities;
using ZestyBiteWebAppSolution.ViewModels;

namespace ZestyBiteWebAppSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller {

        private readonly ZestybiteContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ZestybiteContext context, ILogger<AccountController> logger) {
            _context = context;
            _logger = logger;
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

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn(LogIn logIn) {
            return View(logIn);
        }

        // GET: api/Account
        [HttpGet]
        [Route("Register")]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(Register model) {
            if (model.Password != model.ConfirmPassword) {
                ModelState.AddModelError("ConfirmPassword", "'ConfirmPassword' and 'Password' do not match.");
            }

            // Kiểm tra xem Username đã tồn tại chưa
            var existingUser = await _context.Accounts
                                             .FirstOrDefaultAsync(a => a.UserName == model.Username);
            if (existingUser != null) {
                ModelState.AddModelError("Username", "Username already exists.");
            }

            if (ModelState.IsValid) {
                var account = new Account {
                    UserName = model.Username,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    RoleId = 7,  
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                };

                try {
                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("SignIn", "User");
                } catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
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
