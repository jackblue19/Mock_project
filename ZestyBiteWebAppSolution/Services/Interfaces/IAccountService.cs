using System.Runtime.CompilerServices;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IAccountService {
        Task<IEnumerable<RegisterDTO?>> GetALlAccountAsync();
        Task<Account> CreateStaffAsync(Account account, int roleId);
        Task<RegisterDTO?> GetAccountByIdAsync(int id);
        Task<RegisterDTO?> GetAccountByUsnAsync(string usn);
        Task<RegisterDTO> SignUpAsync(RegisterDTO dto);
        Task<ChangePwdDTO> ChangePwd(ChangePwdDTO dto, string usn);
        Task<UpdateProfileDTO> UpdateProfile(UpdateProfileDTO dto, string usn);
        Task<int> GetRoleIdByUsn(string username);
        Task<bool> IsTrueAccount(string usn, string password);
        Task<string?> GetRoleDescByUsn(string usn);
    }
}
