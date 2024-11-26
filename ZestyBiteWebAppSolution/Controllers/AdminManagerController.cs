using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Services.Interfaces;
using ZestyBiteWebAppSolution.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Humanizer;
using NuGet.Common;

namespace ZestyBiteWebAppSolution.Controllers
{
    public class AdminManagerController : Controller
    {
        public IActionResult Index()
        {
            string? userRole = HttpContext.Session.GetString("RoleDescription");

            if (userRole != "Manager")
            {
                return RedirectToAction("Index", "Home");  // Chuyển hướng về trang chủ nếu không phải Admin
            }

            // Nếu là Admin, hiển thị trang AdminManager
            return View();
        }
    }

}
