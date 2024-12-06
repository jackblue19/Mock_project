using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations {
    public class BillRepository : IBillRepository {
        private readonly ZestyBiteContext _context;
        private readonly IAccountRepository _acc;
        private readonly ITableRepository _tb;

        public BillRepository(ZestyBiteContext context, IAccountRepository acc, ITableRepository tb) {
            _acc = acc;
            _context = context;
            _tb = tb;
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

        public async Task<int> GetLatestBillIdByUsn(string username) {
            Account? accz = await _acc.GetAccountByUsnAsync(username);  // Use await instead of Result to avoid blocking
            int accId = accz.AccountId;

            Table? tbz = await _tb.GetTableByAccIdAsync(accId);  // Use await instead of Result to avoid blocking
            int tbId = tbz.TableId;

            var bilz = await _context.Bills
                                      .Where(boo => boo.TableId == tbId)
                                      .OrderByDescending(boo => boo.BillId)  // Ensure it's ordered deterministically
                                      .FirstOrDefaultAsync();  // Use FirstOrDefaultAsync to handle async operations

            if (bilz != null) {
                return bilz.BillId;
            }

            return 0;  // Return 0 if no bill is found
        }


        /// get latest total cost by usn => sqlraw -> procedure -> execute
    }
}