
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface IRoleRepository : IRepository<Role> {
        Task<Role?> GetRoleIdbyDescription(string desc);
    }
}
