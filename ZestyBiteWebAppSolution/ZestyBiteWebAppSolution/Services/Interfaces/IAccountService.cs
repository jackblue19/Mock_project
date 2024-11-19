using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IAccountService {
        Task<IEnumerable<Account>> GetALlAccountAsync();
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> GetAccountById(int id);
        Task<AccountDTO> SignUpAsync(AccountDTO dto);
    }
}
