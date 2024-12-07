using Microsoft.AspNetCore.Mvc;

namespace ZestyBiteWebAppSolution.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MenuManager()
        {
            return View();
        }
        public IActionResult payHistory()
        {
            return View();
        }
        public IActionResult AccountManagement1()
        {
            return View();
        }


    }
}
