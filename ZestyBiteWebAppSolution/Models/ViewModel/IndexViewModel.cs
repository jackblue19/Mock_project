using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Models.ViewMoedel
{
    public class IndexViewModel {
        public IEnumerable<ItemDTO> PizzaItems { get; set; }
        public IEnumerable<ItemDTO> DrinkItems { get; set; }
        public IEnumerable<ItemDTO> PastaItems { get; set; }
        public IEnumerable<ItemDTO> BurgersItems { get; set; }
        public IEnumerable<ItemDTO> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }


}