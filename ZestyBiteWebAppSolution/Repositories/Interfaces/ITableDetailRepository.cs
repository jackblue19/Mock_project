using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface ITableDetailRepository : IRepository<TableDetail> {
        Task CreateRangeAsync(IEnumerable<TableDetail> tableDetails);
    }
}
