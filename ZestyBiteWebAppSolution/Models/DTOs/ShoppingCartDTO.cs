namespace ZestyBiteWebAppSolution.Models {
    internal class ShoppingCartDTO {
        public List<ShoppingCartItemDTO> Items { get; set; } = new List<ShoppingCartItemDTO>();

        public void AddItem(ShoppingCartItemDTO item) {
            var existingItem = Items.FirstOrDefault(i => i.ItemId == item.ItemId);
            if (existingItem != null) {
                existingItem.Quantity += item.Quantity;
            } else {
                Items.Add(item);
            }
        }

        public void RemoveItem(int itemId) {
            var itemToRemove = Items.FirstOrDefault(i => i.ItemId == itemId);
            if (itemToRemove != null) {
                Items.Remove(itemToRemove);
            }
        }

        public int TotalItems => Items.Sum(i => i.Quantity);

        public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
    }
    public class ShoppingCartItemDTO {
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int Quantity { get; set; }
    }

}
