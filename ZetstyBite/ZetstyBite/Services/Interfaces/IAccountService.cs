using ZetstyBite.Models.Entities;
using ZetstyBite.Models.DTOs;


namespace ZetstyBite.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account?>> GetALlAccountAsync();
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> GetAccountById(int id);
        Task<Account> SignUpAsync(AccountDTO dto);
    }
}
    