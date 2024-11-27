using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDTO>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(int itemid);
        Task<Item> CreateItemAsync(Item item);
        Task<Item> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(int itemid);
    }
}