using ZetstyBite.Models.Entities;

namespace ZetstyBite.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account?>> GetALlAccountAsync();
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> GetAccountById(int id);
        Task<Account> SignUpAsync(Account dto);
    }
}
    