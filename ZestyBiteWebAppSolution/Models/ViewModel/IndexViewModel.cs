using ZestyBiteWebAppSolution.Models.DTOs;

namespace ZestyBiteWebAppSolution.Models.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<ItemDTO> PizzaItems { get; set; } = new List<ItemDTO>();
        public IEnumerable<ItemDTO> DrinkItems { get; set; } = new List<ItemDTO>();
        public IEnumerable<ItemDTO> PastaItems { get; set; } = new List<ItemDTO>();
        public IEnumerable<ItemDTO> BurgersItems { get; set; } = new List<ItemDTO>();
        public IEnumerable<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}