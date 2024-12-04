using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;

namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository, IRepository<Role>
    {
        private readonly ZestyBiteContext _context;
        public RoleRepository(ZestyBiteContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Role?>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }
        public async Task<Role?> GetByIdAsync(Role entity)
        {
            return await _context.Roles.FindAsync(entity.RoleId);
        }
        public async Task<Role> CreateAsync(Role entity)
        {
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Role> UpdateAsync(Role entity)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Role> DeleteAsync(Role entity)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.SingleOrDefaultAsync(r => r.RoleId == id);
        }
    }
}
