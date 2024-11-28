using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class TableDetailRepository : ITableDetailRepository
    {
        private readonly ZestybiteContext _context;

        public TableDetailRepository(ZestybiteContext context)
        {
            _context = context;
        }

        public async Task<TableDetail?> GetTableDetailAsync(int tableId, int itemId)
        {
            return await _context.TableDetails
                                 .Where(td => td.TableId == tableId && td.ItemId == itemId)
                                 .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TableDetail>> GetTableDetailsByTableIdAsync(int tableId)
        {
            return await _context.TableDetails
                                 .Where(td => td.TableId == tableId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<TableDetail>> GetTableDetailsByItemIdAsync(int itemId)
        {
            return await _context.TableDetails
                                 .Where(td => td.ItemId == itemId)
                                 .ToListAsync();
        }

        public async Task<TableDetail> CreateAsync(TableDetail tableDetail)
        {
            _context.TableDetails.Add(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail;
        }

        public async Task<TableDetail> UpdateAsync(TableDetail tableDetail)
        {
            _context.TableDetails.Update(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail;
        }

        public async Task<TableDetail> DeleteAsync(TableDetail tableDetail)
        {
            _context.TableDetails.Remove(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail;
        }

        // Generic repository methods (for example, GetAllAsync, GetByIdAsync) can be reused if needed.
        public async Task<IEnumerable<TableDetail>> GetAllAsync()
        {
            return await _context.TableDetails.ToListAsync();
        }

        public async Task<TableDetail?> GetByIdAsync(int id)
        {
            return await _context.TableDetails
                                 .Where(td => td.TableId == id)
                                 .SingleOrDefaultAsync();
        }
    }
}
