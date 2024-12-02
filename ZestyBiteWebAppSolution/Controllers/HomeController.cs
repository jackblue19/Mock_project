using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.ViewMoedel;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteSolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemService _itemService;
        private readonly ZestyBiteContext _context;

        public HomeController(IItemService itemService, ZestyBiteContext context)
        {
            _itemService = itemService;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var items = await _itemService.GetAllItemsAsync(); // Lấy tất cả các mục

            var pizzaItems = items.Where(i => i.ItemCategory == "Pizza"); // Lọc các món Pizza
            var drinkItems = items.Where(i => i.ItemCategory == "Drink"); // Lọc các món Drink
            var pastaItems = items.Where(i => i.ItemCategory == "Pasta");
            var burgersItems = items.Where(i => i.ItemCategory == "Burgers");

            // Trả về View với hai danh sách: pizza và drink
            var viewModel = new IndexViewModel
            {
                PizzaItems = pizzaItems,
                DrinkItems = drinkItems,
                PastaItems = pastaItems,
                BurgersItems = burgersItems,
            };
            return View(viewModel); // Trả về view với dữ liệu pizza và drink
        }

        public IActionResult About()
        {
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

        public IActionResult Contact()
        {
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

        public async Task<IActionResult> Menu(int page = 1) {
            int pageSize = 6;

            // Get all items from the service
            var itemsDTO = await _itemService.GetAllItemsAsync(); // Lấy tất cả các mục

            // Paginate all items for ftco-section
            int totalItems = itemsDTO.Count();  // Total items across all categories
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedItems = itemsDTO
                .OrderBy(i => i.ItemId)  // Adjust this sorting logic if needed
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Get categorized items (without pagination for ftco-menu)
            var pizzaItems = itemsDTO.Where(i => i.ItemCategory == "Pizza").ToList();
            var drinkItems = itemsDTO.Where(i => i.ItemCategory == "Drink").ToList();
            var pastaItems = itemsDTO.Where(i => i.ItemCategory == "Pasta").ToList();
            var burgersItems = itemsDTO.Where(i => i.ItemCategory == "Burgers").ToList();

            var model = new IndexViewModel {
                PizzaItems = pizzaItems,
                DrinkItems = drinkItems,
                PastaItems = pastaItems,
                BurgersItems = burgersItems,
                Items = paginatedItems,  // The paginated list of items for ftco-section
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
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
