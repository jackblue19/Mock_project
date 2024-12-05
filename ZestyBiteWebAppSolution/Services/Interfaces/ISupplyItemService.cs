using ZestyBiteWebAppSolution.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface ISupplyItemService
    {
        //Task<SupplyItemDTO?> CreateSupplyItemAsync(SupplyItemDTO supplyItemDto);  // Create a new SupplyItem
        //Task<bool> DeleteSupplyItemAsync(int supplyId, int itemId);  // Delete a SupplyItem by SupplyId and ItemId
        Task<IEnumerable<SupplyItemDTO?>> GetAllSupplyItemsAsync();  // Retrieve all SupplyItems
        //Task<SupplyItemDTO?> GetSupplyItemByIdAsync(int supplyId, int itemId);  // Retrieve a SupplyItem by SupplyId and ItemId
        //Task<SupplyItemDTO?> UpdateSupplyItemAsync(SupplyItemDTO supplyItemDto);  // Update an existing SupplyItem
    }
}
