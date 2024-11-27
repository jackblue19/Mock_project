using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface ITableService
    {
        Task<TableDTO> CreateTableAsync(TableDTO dto);
    }
}
