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
    }
}
