using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface IBillRepository {
        Task<Bill?> GetBillAsync(int id);
        Task<Account?> GetNameById(int id);

    }
}
