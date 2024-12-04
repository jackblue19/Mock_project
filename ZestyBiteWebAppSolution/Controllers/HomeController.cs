using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZestyBiteWebAppSolution.Models;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.ViewMoedel;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteSolution.Controllers {
    [AllowAnonymous]
    public class HomeController : Controller {
        private readonly IItemService _itemService;

        public HomeController(IItemService itemService) {
            _itemService = itemService;
        }

        private CheckoutDTO GetShoppingCart() {
            var cart = HttpContext.Session.GetObjectFromJson<CheckoutDTO>("ShoppingCart");
            if (cart == null) {
                cart = new CheckoutDTO();
            }
            return cart;
        }

        public async Task<IActionResult> Index(int? cartTotalItems) {
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

            // Get shopping cart details
            var cart = GetShoppingCart();
            viewModel.TotalItems = cart.Items?.Sum(i => i.Quantity) ?? 0;

            // If cartTotalItems is passed as a query parameter, update the badge count
            if (cartTotalItems.HasValue) {
                viewModel.TotalItems = cartTotalItems.Value;
            }

            return View(viewModel);
        }

        public IActionResult About() {
            return View();
        }

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

            var itemsDTO = await _itemService.GetAllItemsAsync();

            int totalItems = itemsDTO.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedItems = itemsDTO
                .OrderBy(i => i.ItemId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pizzaItems = itemsDTO.Where(i => i.ItemCategory == "Pizza").ToList();
            var drinkItems = itemsDTO.Where(i => i.ItemCategory == "Drink").ToList();
            var pastaItems = itemsDTO.Where(i => i.ItemCategory == "Pasta").ToList();
            var burgersItems = itemsDTO.Where(i => i.ItemCategory == "Burgers").ToList();

            var model = new IndexViewModel {
                PizzaItems = pizzaItems,
                DrinkItems = drinkItems,
                PastaItems = pastaItems,
                BurgersItems = burgersItems,
                Items = paginatedItems,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query, int page = 1) {
            if (string.IsNullOrWhiteSpace(query)) {
                var itemDTO = await _itemService.GetAllItemsAsync();
                return View("SearchResults", itemDTO);
            }

            var itemsDTO = await _itemService.GetAllItemsAsync();
            var filteredItems = itemsDTO
                .Where(i => i.ItemName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            int pageSize = 6;
            int totalItems = filteredItems.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paginatedItems = filteredItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new IndexViewModel {
                Items = paginatedItems,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View("SearchResults", model);
        }
    }
}