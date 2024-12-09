using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface IBillRepository {
        Task<Bill?> CreateAsync(int id);
        Task<int> GetLatestBillIdByUsn(string username);
        Task<Account> GetUsnById(int billid);
        Task<Bill?> UpdateBill(int billId);
        Task<IEnumerable<Bill?>> GetAllAsync();
        Task<Bill?> GetBillByTableId(int billId);
    }
}
