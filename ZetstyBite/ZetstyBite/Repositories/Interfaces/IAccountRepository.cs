using ZetstyBite.Models.Entities;


namespace ZetstyBite.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        //Task<IEnumerable<Account>> GetAllAccountsAsync();
        //Task<Account> GetAccountByIdAsync(int id);
        Task<Account?> GetAccountByUsnAsync(string usn);
        //Task<bool> DeleteAnAccountAsync(int id);
        //Task<Account> UpdateAccountProfileAsync(Account account);
        Task<IEnumerable<Account>> SearchAccountByNamesAsync(string names);
        Task<Account> CreateAccountAsync(Account account);   //  manager only
        Task<Account> AddAsync(Account account);  // customer only
    }
}

/*
    * Commented => Inherited
 */