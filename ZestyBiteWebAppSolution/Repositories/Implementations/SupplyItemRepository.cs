using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Data;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class SupplyItemRepository : ISupplyItemRepository
    {
        private readonly ZestyBiteContext _context;

        public SupplyItemRepository(ZestyBiteContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplyItem?>> GetAllAsync()
        {
            return await _context.SupplyItems
                     .Include(si => si.Item) 
                     .Include(si => si.Supply) 
                     .ToListAsync();
        }

        //public async Task<SupplyItem?> GetByIdAsync(int supplyId, int itemId)
        //{
        //    return await _context.SupplyItems.FindAsync(supplyId, itemId);
        //}

        //public async Task<SupplyItem> CreateAsync(SupplyItem supplyItem)
        //{
        //    if (supplyItem == null)
        //    {
        //        throw new ArgumentNullException(nameof(supplyItem), "SupplyItem cannot be null");
        //    }

        //    _context.SupplyItems.Add(supplyItem);
        //    await _context.SaveChangesAsync();
        //    return supplyItem;
        //}

        //public async Task<SupplyItem> UpdateAsync(SupplyItem supplyItem)
        //{
        //    if (supplyItem == null)
        //    {
        //        throw new ArgumentNullException(nameof(supplyItem), "SupplyItem cannot be null");
        //    }

        //    _context.SupplyItems.Update(supplyItem);
        //    await _context.SaveChangesAsync();
        //    return supplyItem;
        //}

        //public async Task<SupplyItem> DeleteAsync(SupplyItem supplyItem)
        //{
        //    if (supplyItem == null)
        //    {
        //        throw new ArgumentNullException(nameof(supplyItem), "SupplyItem cannot be null");
        //    }

        //    _context.SupplyItems.Remove(supplyItem);
        //    await _context.SaveChangesAsync();
        //    return supplyItem; // Return the deleted entity
        //}
    }
}
