using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface ITableDetailRepository : IRepository<TableDetail> {
        Task CreateRangeAsync(IEnumerable<TableDetail> tableDetails);
        Task<IEnumerable<TableDetail>> GetTableDetailsByAccountIdAsync(int accountId);
        Task<List<TableDetailDTO>> GetByBillIdAsync(int billId);
    }
}