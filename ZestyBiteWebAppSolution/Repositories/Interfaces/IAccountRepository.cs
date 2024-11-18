using ZestyBiteWebAppSolution.Models.Entities;


namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface IAccountRepository {
        Task<Account?> GetAccountByUsnAsync(string usn);
        Task<IEnumerable<Account>> SearchAccountByNamesAsync(string names);
        Task<Account> CreateAccountAsync(Account account);   //  manager only
        Task<Account> AddAsync(Account account);  // customer only
    }
}