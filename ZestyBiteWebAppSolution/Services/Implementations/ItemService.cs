using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository; // Change to IItemRepository

        public ItemService(IItemRepository itemRepository) // Inject IItemRepository
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Item?>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAllAsync();
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

        public async Task<Item> DeleteItemAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id); // Fetch the item
            if (item != null)
            {
                await _itemRepository.DeleteAsync(item); // Delete the item
            }
            return item;
        }
    }
}