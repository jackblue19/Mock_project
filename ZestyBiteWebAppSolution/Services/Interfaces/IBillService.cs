using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IBillService {
        Task<decimal?> CalculateTotalCostAsync();
        Task<IEnumerable<Bill>> GetALlAccAsync();
        Task<List<TableDetailDTO>> GetBillByTableId(int tableId, string usn);
    }
}
