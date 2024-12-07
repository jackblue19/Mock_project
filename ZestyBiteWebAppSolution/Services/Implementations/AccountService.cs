using Microsoft.AspNetCore.Identity;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IRoleRepository _roleRepository;

        public AccountService(IAccountRepository accountRepository, IRoleRepository roleRepository)
        {
            _repository = accountRepository;
            _roleRepository = roleRepository;
        }
        public async Task<Account> CreateStaffAsync(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), "Account was null");

            var existed = await _repository.GetAccountByUsnAsync(account.Username);
            if (existed != null)
                throw new InvalidOperationException($"Username '{account.Username}' is already in use.");

            var created = await _repository.CreateAsync(account);
            return created;
        }

        public async Task<bool> IsVerified(string usn, string code)
        {
            var acc = await _repository.GetAccountByUsnAsync(usn);
            if (acc.VerificationCode != code) return false;
            acc.AccountStatus = 1;
            await _repository.UpdateAsync(acc);
            return true;
        }

        public async Task<bool> IsDeleteUnregistedAccount(string usn)
        {
            try
            {
                Account? acc = await _repository.GetAccountByUsnAsync(usn);
                if (acc == null) return true;
                if (acc.AccountStatus == 0)
                {
                    await _repository.DeleteAsync(acc);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw new InvalidOperationException($"Sth went wrong: {ex.Message}");
            }
        }

        public async Task<RegisterDTO> SignUpAsync(RegisterDTO dto)
        {
            var existed = await _repository.GetAccountByUsnAsync(dto.Username);

            var defaultRole = await _roleRepository.GetByIdAsync(7);
            if (defaultRole == null || defaultRole.RoleId <= 0)
            {
                throw new InvalidOperationException("Invalid role assignment.");
                throw new ArgumentException("Please choose another username!", nameof(dto.Username));
            }
            var acc = new Account()
            {
                Username = dto.Username,
                Password = dto.Password,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Gender = dto.Gender,
                Email = dto.Email,
                ProfileImage = dto.ProfileImg,
                VerificationCode = dto.VerificationCode,
                AccountStatus = 0
            };
            acc.RoleId = defaultRole.RoleId;

            var created = await _repository.CreateAsync(acc);
            dto.VerificationCode = "ai cho ma` xem =D";
            dto.RoleDescription = created.Role.RoleDescription;
            return dto;
        }


        public async Task<IEnumerable<RegisterDTO?>> GetALlAccountAsync()
        {
            var accounts = await _repository.GetAllAsync();
            return accounts.Select(acc => new RegisterDTO
            {
                Username = acc.Username,
                Password = acc.Password,
                Email = acc.Email,
                Name = acc.Name,
                PhoneNumber = acc.PhoneNumber,
                Address = acc.Address,
                Gender = acc.Gender,
                ProfileImg = acc.ProfileImage,
                VerificationCode = "hidden",
                RoleDescription = acc.Role.RoleDescription
            });
        }
        public async Task<RegisterDTO?> GetAccountByIdAsync(int id)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                if (account == null)
                {
                    throw new ArgumentNullException(nameof(account), "Cannot find by id");
                }
                var dto = new RegisterDTO()
                {
                    Username = account.Username,
                    Password = account.Password,
                    Name = account.Name,
                    PhoneNumber = account.PhoneNumber,
                    Address = account.Address,
                    Email = account.Email,
                    Gender = account.Gender,
                    ProfileImg = account.ProfileImage,
                    RoleDescription = account.Role.RoleDescription,
                };
                return dto;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public async Task<ChangePwdDTO> ChangePwd(ChangePwdDTO dto, string usn)
        {
            var current = await _repository.GetAccountByUsnAsync(usn);
            if (current == null)
                throw new InvalidOperationException("Account not found.");
            current.Password = dto.NewPassword;
            await _repository.UpdateAsync(current);
            return dto;
        }
        public async Task<ForgotPwdDTO> NewPwd(ForgotPwdDTO dto, string email)
        {
            var updated = await _repository.GetAccountByEmailAsync(email);
            if (updated == null)
                throw new InvalidOperationException("Email address was not true");
            updated.Password = dto.NewPassword;
            await _repository.UpdateAsync(updated);
            return dto;
        }

        public async Task<ProfileDTO> UpdateProfile(ProfileDTO dto, string usn)
        {
            var current = await _repository.GetAccountByUsnAsync(usn);
            if (current == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            current.Name = dto.Name;
            current.PhoneNumber = dto.PhoneNumber;
            current.Address = dto.Address;
            current.Gender = dto.Gender;
            current.ProfileImage = dto.ProfileImg;
            await _repository.UpdateAsync(current);
            return dto;
        }

        public async Task<RegisterDTO?> GetAccountByUsnAsync(string username)
        {
            var current = await _repository.GetAccountByUsnAsync(username);
            var dto = new RegisterDTO()
            {
                Username = current.Username,
                Password = current.Password,
                Name = current.Name,
                PhoneNumber = current.PhoneNumber,
                Address = current.Address,
                Email = current.Email,
                Gender = current.Gender,
                ProfileImg = current.ProfileImage,
                RoleDescription = current.Role.RoleDescription,
            };
            return dto;
        }

        public async Task<int> GetRoleIdByUsn(string usn)
        {
            var acc = await _repository.GetAccountByUsnAsync(usn);
            return acc.RoleId;
        }
        public async Task<string?> GetRoleDescByUsn(string usn)
        {
            var acc = await _repository.GetAccountByUsnAsync(usn);
            if (acc == null) return null;
            return acc.Role.RoleDescription;
        }
        public async Task<bool> IsTrueAccount(string usn, string pwd)
        {
            var acc = await _repository.GetAccountByUsnAsync(usn);
            if (acc == null) return false;
            if (acc.Password != pwd) return false;
            if (acc.AccountStatus == 0) return false;
            return true;
        }

        public async Task<bool> VerifyOldPasswordAsync(string username, string oldPassword)
        {
            var account = await _repository.GetAccountByUsnAsync(username);
            if (account == null)
                throw new InvalidOperationException("Account not found.");

            return account.Password == oldPassword;
        }


        /* Other method */
        private string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>(); // You can use any object here, e.g., your user model
            return passwordHasher.HashPassword("", password); // Pass null for the user parameter
        }

        public async Task<ProfileDTO> ViewProfileByUsnAsync(string usn)
        {
            var acc = await _repository.GetAccountByUsnAsync(usn);
            var dto = new ProfileDTO()
            {
                Name = acc.Name,
                PhoneNumber = acc.PhoneNumber,
                Email = acc.Email,
                Gender = acc.Gender,
                Address = acc.Address,
                ProfileImg = acc.ProfileImage
            };

            return dto;
        }
        //  manager staff
        public async Task<Account> MapFromDTO(StaffDTO dto)
        {
            var role = await _roleRepository.GetRoleIdbyDescription(dto.RoleDescription);
            var acc = new Account()
            {
                Username = dto.Username,
                Password = dto.Password,
                Name = dto.Fullname,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Gender = dto.Gender,
                Email = dto.Email,
                ProfileImage = dto.ProfileImg,
                VerificationCode = "CreatedByManager",
                AccountStatus = 1,
            };
            acc.RoleId = role.RoleId;
            return acc;
        }
        public async Task<StaffDTO> MapFromEntity(Account acc)
        {
            var dto = new StaffDTO()
            {
                Username = acc.Username,
                Password = acc.Password,
                Fullname = acc.Name,
                PhoneNumber = acc.PhoneNumber,
                Address = acc.Address,
                Gender = acc.Gender,
                Email = acc.Email,
                ProfileImg = acc.ProfileImage,
                RoleDescription = acc.Role.RoleDescription
            };
            return dto;
        }
        public async Task<bool> DeleteAcc(string usn)
        {
            try
            {
                var del = await _repository.GetAccountByUsnAsync(usn);
                if (del == null) throw new InvalidOperationException("Account 404");
                await _repository.DeleteAsync(del);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> ChangeAccStatus(string usn)
        {
            try
            {
                var up = await _repository.GetAccountByUsnAsync(usn);
                if (up == null) throw new InvalidOperationException("Account 404");
                if (up.AccountStatus == 1) up.AccountStatus = 0;
                else if (up.AccountStatus == 0) up.AccountStatus = 1;
                await _repository.UpdateAsync(up);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Account?> GetUsnAsync(string username)
        {
            var current = await _repository.GetAccountByUsnAsync(username);
            return current;
        }
    }
}