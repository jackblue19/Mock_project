using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Helpers;
using ZestyBiteWebAppSolution.Models;
using ZestyBiteWebAppSolution.Models.ViewMoedel;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteSolution.Controllers {
    public class HomeController : Controller {
        private readonly IItemService _itemService;
        private readonly ZestyBiteContext _context;

        public HomeController(IItemService itemService, ZestyBiteContext context) {
            _itemService = itemService;
            _context = context;
        }

        private ShoppingCartDTO GetShoppingCart() {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCartDTO>("ShoppingCart");
            if (cart == null) {
                cart = new ShoppingCartDTO();
            }
            return cart;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var items = await _itemService.GetAllItemsAsync();

            var pizzaItems = items.Where(i => i.ItemCategory == "Pizza").ToList();
            var drinkItems = items.Where(i => i.ItemCategory == "Drink").ToList();
            var pastaItems = items.Where(i => i.ItemCategory == "Pasta").ToList();
            var burgersItems = items.Where(i => i.ItemCategory == "Burgers").ToList();

            var viewModel = new IndexViewModel {
                PizzaItems = pizzaItems,
                DrinkItems = drinkItems,
                PastaItems = pastaItems,
                BurgersItems = burgersItems,
            };

            var cart = GetShoppingCart();
            viewModel.TotalItems = cart.Items?.Sum(i => i.Quantity) ?? 0;

            return View(viewModel);
        }


        public IActionResult About() {
            return View();
        }
        [Authorize]
        public IActionResult Contacto() {
            return View();
        }
        [Authorize(Policy = "UserPolicy")]
        public IActionResult Feeder() {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Feedback() {
            return View();
        }

        public IActionResult Contact() {
            return View();
        }
        public IActionResult Blog() {
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

        [HttpGet]
        public async Task<IActionResult> Search(string query) {
            if (string.IsNullOrWhiteSpace(query)) {
                var itemDTO = await _itemService.GetAllItemsAsync();
                return View("SearchResults", itemDTO);
            }

            var itemsDTO = await _itemService.GetAllItemsAsync();

            var filteredItems = itemsDTO
                .Where(i => i.ItemName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return View("SearchResults", filteredItems);
        }
    }
}