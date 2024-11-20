using System.Runtime.CompilerServices;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IAccountService
    {
        Task<IEnumerable<AccountDTO?>> GetALlAccountAsync();
        Task<Account> CreateStaffAsync(Account account, int roleId); 
        Task<AccountDTO?> GetAccountByIdAsync(int id);  
        Task<AccountDTO?> GetAccountByUsnAsync(string usn);
        Task<AccountDTO> SignUpAsync(AccountDTO dto);
        Task<ChangePwdDTO> ChangePwd(ChangePwdDTO dto);
        Task<UpdateProfileDTO> UpdateProfile(UpdateProfileDTO dto);
        Task<bool> SendVerificationCodeAsync(string email);


    }
}
