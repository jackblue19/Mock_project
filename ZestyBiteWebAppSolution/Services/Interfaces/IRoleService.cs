using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Role?> GetRoleByIdAsync(int id);
    }
}
