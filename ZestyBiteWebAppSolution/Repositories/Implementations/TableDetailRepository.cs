using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class TableDetailRepository : ITableDetailRepository {
        private readonly ZestyBiteContext _context;

        public TableDetailRepository(ZestyBiteContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TableDetail>> GetAllAsync() {
            return await _context.TableDetails.ToListAsync();
        }

        public async Task<TableDetail?> GetByIdAsync(int id) {
            return await _context.TableDetails.FindAsync(id);
        }

        public async Task<TableDetail> CreateAsync(TableDetail tableDetail) {
            if (tableDetail == null) {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            _context.TableDetails.Add(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail;
        }

        public async Task<TableDetail> UpdateAsync(TableDetail tableDetail) {
            if (tableDetail == null) {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            _context.TableDetails.Update(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail;
        }

        public async Task<TableDetail> DeleteAsync(TableDetail tableDetail) {
            if (tableDetail == null) {
                throw new ArgumentNullException(nameof(tableDetail), "TableDetail cannot be null");
            }

            _context.TableDetails.Remove(tableDetail);
            await _context.SaveChangesAsync();
            return tableDetail; // Return the deleted entity
        }

        public async Task CreateRangeAsync(IEnumerable<TableDetail> tableDetails) {
            if (tableDetails == null || !tableDetails.Any()) {
                throw new ArgumentException("No table details provided.");
            }

            await _context.TableDetails.AddRangeAsync(tableDetails);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Table>> GetTableDetailsByAccountIdAsync(int accountId) {
            return await _context.Tables
                .Where(td => td.AccountId == accountId)
                .ToListAsync();
        }

        Task<IEnumerable<TableDetail>> ITableDetailRepository.GetTableDetailsByAccountIdAsync(int accountId) {
            throw new NotImplementedException();
        }
        public async Task<List<TableDetailDTO>> GetByBillIdAsync(int billId) {
            return await _context.TableDetails
                .Where(td => td.BillId == billId)
                .GroupBy(td => new {
                    td.BillId,
                    td.TableId,
                    td.ItemId,
                    td.Item.ItemName,
                    td.Item.SuggestedPrice,
                    td.Quantity
                })
                .Select(group => new TableDetailDTO {
                    BillId = group.Key.BillId,
                    TableId = group.Key.TableId,
                    ItemId = group.Key.ItemId,
                    ItemName = group.Key.ItemName,
                    SuggestedPrice = group.Key.SuggestedPrice,
                    Quantity = group.Key.Quantity
                })
                .ToListAsync();
        }

    }
}