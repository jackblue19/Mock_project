using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class TableRepository : ITableRepository {
        private readonly ZestyBiteContext _context;

        public TableRepository(ZestyBiteContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context), "ZestyBiteContext cannot be null");
        }

        //DEfault CRUD by IRepo
        public async Task<IEnumerable<Table?>> GetAllAsync() {
            return await _context.Tables.ToListAsync();
        }

        public async Task<Table?> GetByIdAsync(int id) {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<Table> CreateAsync(Table table) {
            if (table == null) {
                throw new ArgumentNullException(nameof(table), "Table cannot be null");
            }

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<Table> UpdateAsync(Table table) {
            if (table == null) {
                throw new ArgumentNullException(nameof(table), "Table cannot be null");
            }

            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<Table> DeleteAsync(Table table) {
            if (table == null) {
                throw new ArgumentNullException(nameof(table), "Table cannot be null");
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return table; // Return the deleted entity
        }

        //Addition CRUD
        public async Task<IEnumerable<Table?>> GetTablesByTypeAsync(int tableType) {
            return await _context.Tables.Where(t => t.TableType == tableType).ToListAsync();
        }

        public async Task<Table?> GetTableByAccIdAsync(int idid) {
            var tb = await _context.Tables
                                    .Where(t => t.AccountId == idid)
                                    .OrderByDescending(t => t.TableId)  // Add ordering by TableId or another unique property
                                    .FirstOrDefaultAsync();  // Use FirstOrDefaultAsync instead of LastOrDefault
            return tb;
        }

    }
}