using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations {
    public class BillService : IBillService {
        private readonly ZestyBiteContext _context;
        private readonly IBillRepository _billRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITableRepository _tableRepository;
        private readonly ITableDetailRepository _tableDetailRepository;
        public BillService(ZestyBiteContext context, IBillRepository billRepository, IAccountRepository accountRepository, ITableRepository tableRepository, ITableDetailRepository tableDetailRepository
) {
            _context = context;
            _billRepository = billRepository;
            _accountRepository = accountRepository;
            _tableRepository = tableRepository;
            _tableDetailRepository = tableDetailRepository;
        }

        public async Task<decimal?> CalculateTotalCostAsync() {
            await _context.Database.ExecuteSqlRawAsync("CALL CalculateTotalCost()");

            var totalAmount = await _context.TableDetails
                .Include(td => td.Item)
                .Where(td => td.Item != null)
                .SumAsync(td => td.Item.SuggestedPrice * td.Quantity);

            return totalAmount;
        }
        public async Task<IEnumerable<Bill>> GetALlAccAsync() {
            var accounts = await _billRepository.GetAllAsync();
           return accounts;
        }

        public async Task<List<TableDetailDTO>> GetBillByTableId(int tableId, string usn) {
            try {
                var account = await _accountRepository.GetAccountByUsnAsync(usn);
                if (account == null) {
                    throw new ArgumentNullException(nameof(account), "Cannot find by id");
                }
                int acid = account.AccountId;
                var btb = await _billRepository.GetBillByTableId(tableId);
                var tb  = await _tableRepository.GetTableByAccIdAsync(acid);

                if (tb.AccountId != acid) {
                    return null;
                } else {

                    return await _tableDetailRepository.GetByBillIdAsync(btb.BillId);
                }
            } catch (InvalidOperationException ex) {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
