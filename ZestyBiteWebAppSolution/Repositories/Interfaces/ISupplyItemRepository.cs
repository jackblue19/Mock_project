using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces
{
    public interface ISupplyItemRepository
    {
        Task<IEnumerable<SupplyItem?>> GetAllAsync();  // Retrieve all SupplyItems
        //Task<SupplyItem?> GetByIdAsync(int supplyId, int itemId);  // Retrieve a SupplyItem by SupplyId and ItemId
        //Task<SupplyItem> CreateAsync(SupplyItem supplyItem);  // Create a new SupplyItem
        //Task<SupplyItem> UpdateAsync(SupplyItem supplyItem);  // Update an existing SupplyItem
        //Task<SupplyItem> DeleteAsync(SupplyItem supplyItem);  // Delete a SupplyItem
    }
}
