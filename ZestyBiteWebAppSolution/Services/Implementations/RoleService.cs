using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations {
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository roleRepository)
        {
            _repository = roleRepository;
        }
        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
