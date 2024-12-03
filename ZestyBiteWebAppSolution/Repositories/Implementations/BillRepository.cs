using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations {
    public class BillRepository : IBillRepository {
        private readonly ZestyBiteContext _context;

        public BillRepository(ZestyBiteContext context) {
            _context = context;
        }
        public async Task<Bill?> GetBillAsync(int id) {
            return await _context.Bills
                                  .FirstOrDefaultAsync(acc => acc.BillId == id);
        }
        public async Task<Account?> GetNameById(int id) {
            var bill = await _context.Bills.FirstOrDefaultAsync(p => p.BillId == id);
            var acc = bill.Account;
            return acc;
        }
    }

}
