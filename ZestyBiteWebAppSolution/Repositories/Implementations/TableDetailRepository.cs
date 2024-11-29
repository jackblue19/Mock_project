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

        public async Task<TableDetail?> GetTableDetailByIdAsync(int tableId, int itemId)
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

        public async Task<TableDetail> CreateAsync(TableDetail entity)
        {
            _context.TableDetails.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TableDetail> UpdateAsync(TableDetail entity)
        {
            _context.TableDetails.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TableDetail> DeleteAsync(TableDetail entity)
        {
            _context.TableDetails.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TableDetail>> GetAllAsync()
        {
            return await _context.TableDetails.ToListAsync();
        }

        public Task<TableDetail?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

