using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        private ItemDTO? MapToItemDTO(Item item)
        {
            if (item == null)
                return null;
            return new ItemDTO
            {
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

        private async Task<IEnumerable<ItemDTO>> GetItemsByCategoryAsync(string category)
        {
            var items = await _itemRepository.GetAllAsync(); // Lấy tất cả các món ăn
            var categoryItems = items.Where(i => i?.ItemCategory == category); // Lọc theo danh mục

            return categoryItems.Select(item => new ItemDTO
            {
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


        public async Task<IEnumerable<ItemDTO>> GetDrinkItemsAsync()
        {
            return await GetItemsByCategoryAsync("Drink");
        }

        public async Task<IEnumerable<ItemDTO>> GetBurgersItemsAsync()
        {
            return await GetItemsByCategoryAsync("Burgers");
        }

        public async Task<IEnumerable<ItemDTO>> GetPastaItemsAsync()
        {
            return await GetItemsByCategoryAsync("Pasta");
        }


        public async Task<IEnumerable<ItemDTO?>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(MapToItemDTO).Where(item => item != null).ToList();
        }


        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _itemRepository.GetByIdAsync(id);
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            return await _itemRepository.CreateAsync(item);
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            return await _itemRepository.UpdateAsync(item);
        }

        public async Task<bool> DeleteItemAsync(int itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item != null)
            {
                await _itemRepository.DeleteAsync(item);
                return true;
            }
            return false;
        }

        /*  CRUD Item By Manager    */
        public async Task<IEnumerable<EItemDTO?>> ViewAllItem()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(eis => new EItemDTO
            {
                ItemId = eis.ItemId,
                IsServed = eis.IsServed,
                ItemCategory = eis.ItemCategory,
                ItemDescription = eis.ItemDescription,
                ItemImage = eis.ItemImage,
                ItemName = eis.ItemName,
                ItemStatus = eis.ItemStatus,
                OriginalPrice = eis.OriginalPrice,
                SuggestedPrice = eis.SuggestedPrice
            }).ToList();
        }

        public async Task<EItemDTO> ViewOneDish(int itemId)
        {
            var eis = await _itemRepository.GetByIdAsync(itemId);
            var dto = new EItemDTO()
            {
                ItemId = eis.ItemId,
                IsServed = eis.IsServed,
                ItemCategory = eis.ItemCategory,
                ItemDescription = eis.ItemDescription,
                ItemImage = eis.ItemImage,
                ItemName = eis.ItemName,
                ItemStatus = eis.ItemStatus,
                OriginalPrice = eis.OriginalPrice,
                SuggestedPrice = eis.SuggestedPrice
            };
            return dto;
        }

        public async Task<Item> CreateNewDish(EItemDTO dto)
        {
            var eis = new Item()
            {
                ItemId = dto.ItemId,
                IsServed = dto.IsServed,
                ItemCategory = dto.ItemCategory,
                ItemDescription = dto.ItemDescription,
                OriginalPrice = dto.OriginalPrice,
                ItemImage = dto.ItemImage,
                ItemName = dto.ItemName,
                ItemStatus = dto.ItemStatus,
                SuggestedPrice = dto.SuggestedPrice
            };
            await _itemRepository.CreateAsync(eis);
            return eis;
        }

        public async Task<Item> ModifyDish(EItemDTO dto)
        {
            var eis = await _itemRepository.GetByIdAsync(dto.ItemId);
            eis.IsServed = dto.IsServed;
            eis.ItemCategory = dto.ItemCategory;
            eis.ItemDescription = dto.ItemDescription;
            eis.OriginalPrice = dto.OriginalPrice;
            eis.SuggestedPrice = dto.SuggestedPrice;
            eis.ItemImage = dto.ItemImage;
            eis.ItemName = dto.ItemName;
            eis.ItemStatus = dto.ItemStatus;
            await _itemRepository.UpdateAsync(eis);
            return eis;
        }
        // DeleteItemAsync
    }
}
