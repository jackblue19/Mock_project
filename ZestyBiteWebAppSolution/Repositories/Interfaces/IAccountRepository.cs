using ZestyBiteWebAppSolution.Models.Entities;


namespace ZestyBiteWebAppSolution.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> GetAccountByUsnAsync(string usn);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<IEnumerable<Account?>> SearchAccountByNamesAsync(string name);
        Task<Account> CreateAccountAsync(Account account , int roleId);   //  manager only
    }
}