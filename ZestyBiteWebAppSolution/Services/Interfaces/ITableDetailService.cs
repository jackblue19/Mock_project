using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface ITableDetailService
    {
        Task<IEnumerable<TableDetail?>> GetAllTableDetailsAsync();
        Task<TableDetail?> GetTableDetailByIdAsync(int tableId, int itemId);
        Task<TableDetail> CreateTableDetailAsync(TableDetail tableDetail);
        Task<TableDetail> UpdateTableDetailAsync(int tableId, int itemId, TableDetail tableDetail);
        Task<bool> DeleteTableDetailAsync(int tableId, int itemId);
    }
}
