using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface ITableDetailService {
        Task<TableDetailDTO?> CreateTableDetailAsync(TableDetailDTO tableDetaildto);
        Task<bool> DeleteTableDetailAsync(int tableDetailId);
        Task<IEnumerable<TableDetailDTO?>> GetAllTableDetailsAsync();
        Task<TableDetailDTO?> GetTableDetailByIdAsync(int tableDetailId);
        Task<TableDetailDTO?> UpdateTableDetailAsync(TableDetailDTO tableDetaildto);
        Task<int> ToPayment(Dictionary<int?, int?> itemQuantityMap, Account acc, string CartSessionKey, HttpContext httpContext);
    }
}