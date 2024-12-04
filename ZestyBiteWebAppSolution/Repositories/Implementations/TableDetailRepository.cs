using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class TableDetailRepository : ITableDetailRepository
    {
        private readonly ZestyBiteContext _context;

        public TableDetailRepository(ZestyBiteContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TableDetail?>> GetAllAsync()
        {
            return await _context.TableDetails.ToListAsync();
        }

        public async Task<TableDetail?> GetByIdAsync(int id)
        {
            return await _context.TableDetails.FindAsync(id);
        }

        public async Task<TableDetail> CreateAsync(TableDetail tableDetail)
        {
            if (tableDetail == null)
            {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            _context.TableDetails.Add(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail;
        }

        public async Task<TableDetail> UpdateAsync(TableDetail tableDetail)
        {
            if (tableDetail == null)
            {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            _context.TableDetails.Update(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail;
        }

        public async Task<TableDetail> DeleteAsync(TableDetail tableDetail)
        {
            if (tableDetail == null)
            {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            _context.TableDetails.Remove(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail; // Return the deleted entity
        }
    }
}