using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface ITableDetailService {
        Task<bool> DeleteTableDetailAsync(int tableDetailId);
        Task<IEnumerable<TableDetailDTO?>> GetAllTableDetailsAsync();
        Task<TableDetailDTO?> UpdateTableDetailAsync(TableDetailDTO tableDetaildto);
        Task<int> ToPayment(Dictionary<int, int> itemQuantityMap, Account acc, string CartSessionKey, HttpContext httpContext);
    }
}