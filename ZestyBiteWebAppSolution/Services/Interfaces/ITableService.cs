using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface ITableService
    {
        Task<Table> CreateTableAsync(Table table);
        Task<Table?> GetTableByIdAsync(int tableId);
        Task<IEnumerable<Table>> GetAllTablesAsync();
        Task<Table> UpdateTableAsync(Table table);
        Task<bool> DeleteTableAsync(int tableId);
    }
}