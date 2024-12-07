using Microsoft.EntityFrameworkCore;
//using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Data;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class SupplyRepository : ISupplyRepository
    {
        private readonly ZestyBiteContext _context;

        public SupplyRepository(ZestyBiteContext context)
        {
            _context = context;
        }

        public async Task<Supply?> GetSupplyByIdAsync(int id)
        {
            return await _context.Supplies.FindAsync(id);
        }
        public async Task<Supply?> GetSupplyByTableIdAsync(int tableId)
        {
            return await _context.Supplies
                                 .Where(s => s.TableId == tableId)
                                 .FirstOrDefaultAsync();
        }
        //public async Task<Supply?> GetByVendorPhoneAsync(string vendorPhone)
        //{
        //    return await _context.Supplies
        //                         .Where(s => s.VendorPhone == vendorPhone)
        //                         .FirstOrDefaultAsync();
        //}

        public async Task<IEnumerable<Supply>> GetAllAsync()
        {
            return await _context.Supplies.ToListAsync();
        }

        public async Task<Supply> UpdateSupplyAsync(Supply entity)
        {
            _context.Supplies.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        //public async Task<Supply?> GetByIdAsync(int id)
        //{
        //    return await _context.Supplies
        //                         .Include(s => s.TableId)
        //                         .FirstOrDefaultAsync(s => s.SupplyId == id);
        //}
        //public async Task<Supply?> GetSupplyByProductNameAsync(string productName)
        //{
        //    return await _context.Supplies
        //                         .FirstOrDefaultAsync(s => s.ProductName == productName);
        //}

        public async Task<Supply> CreateSupplyAsync(Supply supply)
        {
            _context.Supplies.Add(supply);
            await _context.SaveChangesAsync();
            return supply;
        }
        public async Task<Supply> DeleteAsync(Supply supply)
        {
            _context.Supplies.Remove(supply);
            await _context.SaveChangesAsync();
            return supply;
        }
    }

}
