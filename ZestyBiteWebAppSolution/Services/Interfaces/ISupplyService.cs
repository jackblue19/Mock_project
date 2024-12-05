using ZestyBiteWebAppSolution.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface ISupplyService
    {
        // Method to retrieve all supplies
        Task<IEnumerable<SupplyDTO>> GetAllSuppliesAsync();

        // Method to retrieve a supply by ID
        Task<SupplyDTO?> GetSupplyByIdAsync(int id);

        //// Method to create a new supply
        Task<SupplyDTO> CreateSupplyAsync(SupplyDTO dto);
        Task<SupplyDTO?> GetSupplyByTableIdAsync(int tableId);


        //// Method to update an existing supply
        Task<SupplyDTO?> UpdateSupplyAsync(SupplyDTO dto, int id);

        // Method to delete a supply
        Task<bool> DeleteSupplyAsync(int supplyId);
    }
}
