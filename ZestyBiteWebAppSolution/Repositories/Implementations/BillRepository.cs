using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
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
            Account? accz = await _acc.GetAccountByUsnAsync(username);  
            int accId = accz.AccountId;

            Table? tbz = await _tb.GetTableByAccIdAsync(accId);  
            int tbId = tbz.TableId;

            var bilz = await _context.Bills
                                      .Where(boo => boo.TableId == tbId)
                                      .OrderByDescending(boo => boo.BillId)  
                                      .FirstOrDefaultAsync();  

            if (bilz != null) {
                return bilz.BillId;
            }

            return 0;  // Return 0 if no bill is found
        }
        public async Task<Account> GetUsnById(int billid) {
            var bbb = await _context.Bills.FirstOrDefaultAsync(p => p.BillId == billid);
            var tb = await _tb.GetByIdAsync(bbb.TableId);
            var acc = await _acc.GetByIdAsync(tb.AccountId);
            return acc;
        }

        public async Task<Bill?> GetBillById (int billId) {
            return  _context.Bills.FirstOrDefault(p => p.BillId == billId);
        }

        public async Task<Bill?> UpdateBill(int billId) {
            var b = _context.Bills.FirstOrDefault(p => p.BillId == billId);
            b.BillStatus = 1;
            _context.Bills.Update(b);
            await _context.SaveChangesAsync();
            return b;
        }

    }
}