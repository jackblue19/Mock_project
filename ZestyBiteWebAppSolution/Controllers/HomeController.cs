using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models.ViewMoedel;
using ZestyBiteWebAppSolution.Services.Interfaces;


namespace ZestyBiteSolution.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        public HomeController(IAccountService accountService)
        {
            _accountService = accountService;
namespace ZestyBiteSolution.Controllers {
    public class HomeController : Controller {
        private readonly IItemService _itemService;
        public HomeController( IItemService itemService) {
            _itemService = itemService;
        }

        public async Task<IActionResult> Index() {
            var items = await _itemService.GetAllItemsAsync(); // Lấy tất cả các mục

            var pizzaItems = items.Where(i => i.ItemCategory == "Pizza"); // Lọc các món Pizza
            var drinkItems = items.Where(i => i.ItemCategory == "Drink"); // Lọc các món Drink

            // Trả về View với hai danh sách: pizza và drink
            var viewModel = new IndexViewModel {
                PizzaItems = pizzaItems,
                DrinkItems = drinkItems
            };

            return View(viewModel); // Trả về view với dữ liệu pizza và drink
        }


        public IActionResult About() {
            return View();
        }
        [Authorize]     // => force to login but dont care role
        public IActionResult Contacto()
        {
            return View();
        }
        [Authorize(Policy = "UserPolicy")] // => sử dụng policy từ program.cs để cho gọn =)))
        public IActionResult Feeder()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Feedback()
        {
            return View();
        }

        public IActionResult Contact() {
            return View();
        }

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
