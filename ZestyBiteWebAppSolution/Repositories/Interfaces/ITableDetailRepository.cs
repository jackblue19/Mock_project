using Microsoft.EntityFrameworkCore.Metadata;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces
{
    public interface ITableDetailRepository : IRepository<TableDetail>
    {
        Task<TableDetail?> GetTableDetailAsync(int tableId, int itemId);
        Task<IEnumerable<TableDetail>> GetTableDetailsByTableIdAsync(int tableId);
        Task<IEnumerable<TableDetail>> GetTableDetailsByItemIdAsync(int itemId);
    }
}
