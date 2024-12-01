using ZestyBiteWebAppSolution.Models.DTOs;

namespace ZestyBiteWebAppSolution.Models.ViewMoedel
{
    internal class IndexViewModel
    {
        public IEnumerable<ItemDTO> PizzaItems { get; set; }
        public IEnumerable<ItemDTO> DrinkItems { get; set; }
    }
}