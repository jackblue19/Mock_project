using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces
{
    public interface ITableRepository
    {
        Task<Table> CreateAsync(Table table);
        Task<Table?> GetByIdAsync(int tableId);
        Task<IEnumerable<Table>> GetAllAsync();
        Task<Table> UpdateAsync(Table table);
        Task<bool> DeleteAsync(int tableId);
    }
}