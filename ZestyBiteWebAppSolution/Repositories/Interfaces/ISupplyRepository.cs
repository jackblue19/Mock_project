using ZestyBiteWebAppSolution.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZestyBiteWebAppSolution.Models.DTOs;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces
{
    public interface ISupplyRepository
    {
        // Method to retrieve all supplies
        Task<IEnumerable<Supply>> GetAllAsync();

        // Method to retrieve a supply by ID
        Task<Supply?> GetSupplyByIdAsync(int id);
        Task<Supply?> GetByVendorPhoneAsync(string vendorPhone);

        Task<Supply> CreateSupplyAsync(Supply supply);

        //// Method to update an existing supply
        Task<Supply> UpdateSupplyAsync(Supply supply);
        Task<Supply?> GetSupplyByTableIdAsync(int tableId);

        //// Method to delete a supply
        Task<Supply> DeleteAsync(Supply supply);
    }
}
