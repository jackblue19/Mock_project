using System.Runtime.CompilerServices;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IAccountService {
        Task<IEnumerable<RegisterDTO?>> GetALlAccountAsync();
        Task<IEnumerable<AccountDTO?>> GetALlAccAsync();
        Task<Account> CreateStaffAsync(Account account);
        Task<RegisterDTO?> GetAccountByIdAsync(int id);
        Task<RegisterDTO?> GetAccountByUsnAsync(string usn);
        Task<RegisterDTO> SignUpAsync(RegisterDTO dto);
        Task<ChangePwdDTO> ChangePwd(ChangePwdDTO dto, string usn);
        Task<bool> NewPwd(ForgotPwdDTO dto, string usn);
        Task<ProfileDTO> UpdateProfile(ProfileDTO dto, string usn);
        Task<int> GetRoleIdByUsn(string username);
        Task<bool> IsTrueAccount(string usn, string password);
        Task<string?> GetRoleDescByUsn(string usn);

        Task<ProfileDTO> ViewProfileByUsnAsync(string usn);
        Task<bool> IsVerified(string usn, string code);
        Task<bool> IsDeleteUnregistedAccount(string usn);
        Task<bool> VerifyOldPasswordAsync(string username, string oldPassword);
        Task<Account?> GetUsnAsync(string username);
        Task<Account> GetByUsn(string username);
        Task<Account> MapFromDTO(StaffDTO dto);
        Task<StaffDTO> MapFromEntity(Account acc);
        Task<bool> DeleteAcc(string usn);
        Task<bool> ChangeAccStatus(string usn);
        Task<Account?> GetAccByMailAsync(string mail);
        Task<Account> UpdateVCode(Account dto);
    }
}