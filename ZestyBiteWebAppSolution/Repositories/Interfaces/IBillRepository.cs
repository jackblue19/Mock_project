using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface IBillRepository {
        Task<Bill?> CreateAsync(int id);
        Task<int> GetLatestBillIdByUsn(string username);

    }
}
