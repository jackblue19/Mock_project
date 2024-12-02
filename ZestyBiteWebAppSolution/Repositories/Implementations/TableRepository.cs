using ZestyBiteWebAppSolution.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class TableRepository : ITableRepository
    {
        private readonly ZestyBiteContext _context;

        public TableRepository(ZestyBiteContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Table> CreateAsync(Table table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
            return table;
        }

        // Read
        public async Task<Table?> GetByIdAsync(int tableId)
        {
            return await _context.Tables
                                 .Where(t => t.TableId == tableId)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        // Update
        public async Task<Table> UpdateAsync(Table table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
            return table;
        }

        // Delete
        public async Task<bool> DeleteAsync(int tableId)
        {
            var table = await GetByIdAsync(tableId);
            if (table == null)
                return false;

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}