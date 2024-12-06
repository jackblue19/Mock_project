using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations {
    public class BillService : IBillService {
        private readonly ZestyBiteContext _context;

        public BillService(ZestyBiteContext context) {
            _context = context;
        }

        public async Task<decimal?> CalculateTotalCostAsync() {
            await _context.Database.ExecuteSqlRawAsync("CALL CalculateTotalCost()");

            var totalAmount = await _context.TableDetails
                .Include(td => td.Item)
                .Where(td => td.Item != null)
                .SumAsync(td => td.Item.SuggestedPrice * td.Quantity);

            return totalAmount;
        }
    }
}
