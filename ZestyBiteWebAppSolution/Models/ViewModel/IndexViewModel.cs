using ZestyBiteWebAppSolution.Models.DTOs;

namespace ZestyBiteWebAppSolution.Models.ViewMoedel {
    public class IndexViewModel {
        public IEnumerable<ItemDTO> PizzaItems { get; set; } = null!;
        public IEnumerable<ItemDTO> DrinkItems { get; set; } = null!;
        public IEnumerable<ItemDTO> PastaItems { get; set; } = null!;
        public IEnumerable<ItemDTO> BurgersItems { get; set; } = null!;
        public IEnumerable<ItemDTO> Items { get; set; } = null!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

    }
}