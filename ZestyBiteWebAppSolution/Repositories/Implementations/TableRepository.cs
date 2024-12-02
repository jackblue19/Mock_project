using ZestyBiteWebAppSolution.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Data;

namespace ZestyBiteWebAppSolution.Repositories
{
    public class TableRepository
    {
        private readonly ZestyBiteContext _context;

        public TableRepository(ZestyBiteContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Table> CreateTableAsync(Table table)
        {
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return table;
        }

        // Read
        public async Task<Table> GetTableByIdAsync(int tableId)
        {
            return await _context.Tables
                .Include(t => t.Account)
                .Include(t => t.Item)
                .Include(t => t.Reservation)
                .FirstOrDefaultAsync(t => t.TableId == tableId);
        }

        public async Task<List<Table>> GetAllTablesAsync()
        {
            return await _context.Tables
                .Include(t => t.Account)
                .Include(t => t.Item)
                .Include(t => t.Reservation)
                .ToListAsync();
        }

        // Update
        public async Task<Table> UpdateTableAsync(Table table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
            return table;
        }

        // Delete
        public async Task<bool> DeleteTableAsync(int tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null) return false;

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
