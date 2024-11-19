using ZetstyBite.Data;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using ZetstyBite.Repositories.Interfaces;
using ZetstyBite.Models.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace ZetstyBite.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository, IRepository<Role>
    {
        private readonly ZestyBiteDbContext _context;
        public RoleRepository(ZestyBiteDbContext context)
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
        public async Task<Role> DeletedAsync(Role entity)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Role> GetById(int id)
        {
            return await _context.Roles.SingleOrDefaultAsync(r => r.RoleId == id);
        }
    }
}