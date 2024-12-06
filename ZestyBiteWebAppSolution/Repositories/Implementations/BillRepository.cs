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
        public async Task<Bill> GetBillByTableId(int tableId) {
            var ab =  _context.Bills.Include(p => p.Table).FirstOrDefault(p => p.TableId== tableId);
            return ab;
        }
    }
}