using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDTO?>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(int id);
        Task<Item> CreateItemAsync(Item item);
        Task<Item> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(int itemId);

        Task<IEnumerable<ItemDTO>> GetDrinkItemsAsync();
        Task<IEnumerable<ItemDTO>> GetBurgersItemsAsync();
        Task<IEnumerable<ItemDTO>> GetPastaItemsAsync();

        /*CRUD for manager*/
        Task<IEnumerable<EItemDTO?>> ViewAllItem();
        Task<EItemDTO> ViewOneDish(int itemId);
        Task<Item> CreateNewDish(EItemDTO dto);
        Task<Item> ModifyDish(EItemDTO dto);

    }

}