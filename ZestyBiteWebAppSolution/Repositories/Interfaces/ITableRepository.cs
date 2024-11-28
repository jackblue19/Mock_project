using Microsoft.EntityFrameworkCore.Metadata;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces
{
    public interface ITableRepository: IRepository<Table>
    {
        Task<Table?> GetTableByIdAsync(int tableId);
        Task<IEnumerable<Table>> GetTablesByAccountIdAsync(int accountId);
        Task<IEnumerable<Table>> GetAvailableTablesAsync();
    }
}
