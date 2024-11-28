using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface ITableService
    {
        Task<IEnumerable<Table?>> GetAllTablesAsync();
        Task<Table?> GetTableByIdAsync(int id);
        Task<Table> CreateTableAsync(Table table);
        Task<Table> UpdateTableAsync(int id, Table table);
        Task<bool> DeleteTableAsync(int id);
    }
}
