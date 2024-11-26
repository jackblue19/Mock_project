using Microsoft.EntityFrameworkCore;
using ZetstyBite.Models.Entities;
using ZetstyBite.Repositories.Interfaces;
using ZetstyBite.Services.Interfaces;
using Humanizer;

namespace ZetstyBite.Services.Implementations
{
    public class RoleService : IRoleService{
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository roleRepository)
        {
            _repository = roleRepository;
        }
        public async Task<Role?> GetRoleByIdAsync(int id){
            return await _repository.GetById(id);
        }
    }
}