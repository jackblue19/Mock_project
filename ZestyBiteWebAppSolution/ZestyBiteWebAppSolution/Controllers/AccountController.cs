using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller {
        private readonly IAccountService _service;
        private readonly ZestybiteContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ZestybiteContext context, ILogger<AccountController> logger, IAccountService accountService) {
            _context = context;
            _logger = logger;
            _service = accountService;
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
            if (accountDto == null) return BadRequest(new { Message = "Invalid payload" });

            try {
                var created = await _service.SignUpAsync(accountDto);
                return Created($"/api/account/{created.Id}", created);
            } catch (InvalidOperationException ex) {
                return BadRequest(new { Message = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500, new { Message = "Internal Server Error", Detail = ex.Message });
            }
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
