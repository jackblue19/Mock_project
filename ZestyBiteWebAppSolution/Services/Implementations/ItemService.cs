using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations {
    public class ItemService : IItemService {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository) {
            _itemRepository = itemRepository;
        }

        private ItemDTO? MapToItemDTO(Item? item) {
            if (item == null) return null;

            return new ItemDTO {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                SuggestedPrice = item.SuggestedPrice,
                ItemCategory = item.ItemCategory,
                ItemStatus = item.ItemStatus,
                ItemImage = item.ItemImage,
                IsServed = item.IsServed
            };
        }

        public async Task<IEnumerable<ItemDTO>> GetDrinkItemsAsync() {
            var items = await _itemRepository.GetAllAsync(); // Lấy tất cả các món ăn
            var drinkItems = items.Where(i => i.ItemCategory == "Drink"); // Lọc chỉ món Drink

            // Chuyển đổi các mục Drink sang ItemDTO và trả về
            return drinkItems.Select(item => new ItemDTO {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                SuggestedPrice = item.SuggestedPrice,
                ItemCategory = item.ItemCategory,
                ItemStatus = item.ItemStatus,
                ItemImage = item.ItemImage,
                IsServed = item.IsServed
            }).ToList();
        }


        public async Task<IEnumerable<ItemDTO?>> GetAllItemsAsync() {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(MapToItemDTO);
        }

        public async Task<Item?> GetItemByIdAsync(int id) {
            return await _itemRepository.GetByIdAsync(id);
        }

        public async Task<Item> CreateItemAsync(Item item) {
            return await _itemRepository.CreateAsync(item);
        }

        public async Task<Item> UpdateItemAsync(Item item) {
            return await _itemRepository.UpdateAsync(item);
        }

        public async Task<bool> DeleteItemAsync(int itemId) {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item != null) {
                await _itemRepository.DeleteAsync(item);
                return true; // Return true if the item was found and deleted
            }
            return false; // Return false if the item was not found
        }

        Task IItemService.GetDrinkItemsAsync() {
            throw new NotImplementedException();
        }
    }
}
