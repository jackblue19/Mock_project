using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IAccountService
    {
        Task<IEnumerable<Account?>> GetALlAccountAsync();
        Task<Account> CreateStaffAsync(Account account, int roleId); 
        Task<Account?> GetAccountById(int id);   
        Task<AccountDTO> SignUpAsync(AccountDTO dto);
    }
}
