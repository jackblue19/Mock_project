using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class BillRepository : IBillRepository {
        private readonly ZestyBiteContext _context;

        public BillRepository(ZestyBiteContext context) {
            _context = context;
        }

        public async Task<Bill> CreateAsync(int tableId) {
            var ab = new Bill() {
                BillStatus = 0,
                PaymentId = 1,
                TableId = tableId,
                TotalCost = 100000,
                BillDatetime = DateTime.Now,
                BillType = 1,
            };
            _context.Bills.Add(ab);
            _context.SaveChanges();
            return ab;
        }

        //public async Task<IEnumerable<TableDetail>> GetTableDetailsByAccountIdAsync(int accountId) {
        //    var bill = await _context.Bills
        //                             .Include(b => b.TableDetails)
        //                             .ThenInclude(td => td.Item) // Include the related Item
        //                             .FirstOrDefaultAsync(b => b.AccountId == accountId);

        //    if (bill == null) return Enumerable.Empty<TableDetail>();

        //    return bill.TableDetails;
        //}


        //public async Task<Account?> GetNameById(int id) {
        //    var bill = await _context.Bills.FirstOrDefaultAsync(p => p.BillId == id);
        //    var acc = bill?.Account;
        //    return acc;
        //}
    }
}