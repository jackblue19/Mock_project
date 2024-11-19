using ZetstyBite.Models.Entities;


namespace ZetstyBite.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task<Role> GetById(int id);
    }
}