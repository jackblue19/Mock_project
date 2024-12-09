using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface ITableRepository : IRepository<Table> {
        Task<IEnumerable<Table?>> GetTablesByTypeAsync(int tableType);
        Task<Table?> GetTableByAccIdAsync(int idid);
    }
}