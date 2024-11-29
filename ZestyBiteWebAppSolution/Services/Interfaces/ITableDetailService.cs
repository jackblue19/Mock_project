using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface ITableDetailService
    {
        Task<TableDetail?> GetTableDetailByIdAsync(int tableId, int itemId);
    }
}
