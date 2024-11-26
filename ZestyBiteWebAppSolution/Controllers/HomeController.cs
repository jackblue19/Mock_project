using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Services.Interfaces;
using ZestyBiteWebAppSolution.Services.Implementations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


namespace ZestyBiteSolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        public HomeController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        //  aps dunjg cac ham cho controller khac 
        public IActionResult Index()
        {
            string? userName = HttpContext.Session.GetString("Username");
            string? roleId = HttpContext.Session.GetString("RoleId");
            string? userRole = HttpContext.Session.GetString("RoleDescription");
            // giờ là chia theo các case
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("LogIn", "Account"); // => require logi to use some func
            }
            else
            {
                /*https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-return-a-value-from-a-task*/
                var id = _accountService.GetRoleIdByUsn(userName).Result;   //  using .Result -> return a value from a Task
                switch (id)
                {
                    case 1:
                        {
                            return RedirectToAction("Home", "Account?");
                        }
                    case 2:
                        {
                            return RedirectToAction("Manager", "Accoutn?");
                        }
                        /* => case 3 4 5 6  7 -> orrder taker, user , .... */
                }
            }

            ViewBag.UserName = userName;
            ViewBag.RoleId = roleId;
            ViewBag.UserRole = userRole;

            return View();
        }
        [HttpGet("protected")]
        [Authorize]  // Yêu cầu người dùng phải xác thực (JWT Token hợp lệ)
        public IActionResult GetProtectedData()
        {
            return Ok("This is a protected endpoint");
        }
        // Chỉ người dùng có vai trò "manager" hoặc "staff" mới có quyền truy cập trang này
        [Authorize(Roles = "manager,staff")]
        public IActionResult AdminPanel()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            var userName = User.Identity.Name;  // Truy xuất tên người dùng từ claim
            // var userRole = User.FindFirst(ClaimTypes.Role)?.Value;  // Truy xuất vai trò người dùng từ claim

            return View();
        }

        // no [] => no login required as well as no role needed
        public IActionResult About()
        {
            return View();
        }
        [Authorize]     // => force to login but dont care role
        public IActionResult Contact()
        {
            return View();
        }
        [Authorize(Policy = "UserPolicy")] // => sử dụng policy từ program.cs để cho gọn =)))
        public IActionResult Feedback()
        {
            return View();
        }

        /*  demo usage of parameter Id from program.cs of MapControllerRoute
        public class ProductController : Controller
        {
            public IActionResult List()
            {
                return View();
            }

            public IActionResult Details(int id)
            {
                return View(id);
            }
        }

        */

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Search()
        {
            return PartialView();
        }

        public IActionResult BookTable()
        {
            TempData["ShowPopup"] = true;
            return View();
        }

    }
}
