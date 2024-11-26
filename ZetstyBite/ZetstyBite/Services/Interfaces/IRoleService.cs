using ZetstyBite.Models.Entities;

namespace ZetstyBite.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Role?> GetRoleByIdAsync(int id);
    }
}
    