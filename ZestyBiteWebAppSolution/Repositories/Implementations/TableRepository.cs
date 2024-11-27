using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class TableRepository : ITableRepository, IRepository<Table>
    {
        private readonly ZestybiteContext _context;
        public TableRepository(ZestybiteContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Table?>> GetAllAsync()
        {
            return await _context.Tables.ToListAsync();
        }
        public async Task<Table?> GetByIdAsync(Table entity)
        {
            return await _context.Tables.FindAsync(entity.TableId);
        }
        public async Task<Table> CreateAsync(Table entity)
        {
            _context.Tables.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Table> UpdateAsync(Table entity)
        {
            _context.Tables.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Table> DeleteAsync(Table entity)
        {
            _context.Tables.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Table?> GetByIdAsync(int id)
        {
            return await _context.Tables.SingleOrDefaultAsync(r => r.TableId == id);
        }
    }
}
