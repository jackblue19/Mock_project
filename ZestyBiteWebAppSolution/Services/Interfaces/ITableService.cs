using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface ITableService
    {
        //Default
        Task<TableDTO?> CreateTableAsync(TableDTO tableDto);
        Task<bool> DeleteTableAsync(int tableId);
        Task<IEnumerable<TableDTO?>> GetAllTablesAsync();
        Task<TableDTO?> GetTableByIdAsync(int tableId);
        Task<TableDTO?> UpdateTableAsync(TableDTO tableDto);

        //Addition
        Task<IEnumerable<TableDTO?>> GetAllRealTablesAsync();
        Task<IEnumerable<TableDTO?>> GetAllVirtualTablesAsync();
        Task<IEnumerable<TableDetailDTO>> GetTableItemsByTableIdAsync(int tableId);
    }
}