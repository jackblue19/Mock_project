using Microsoft.AspNetCore.Identity;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;
using ZestyBiteWebAppSolution.Helpers;

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
        public async Task<Account> CreateStaffAsync(Account account, int roleId)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            }

            var existed = await _repository.GetAccountByUsnAsync(account.Username);
            if (existed != null)
            {
                throw new InvalidOperationException($"Username '{account.Username}' is already in use.");
            }

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
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Input account was null.");
            }

            if (string.IsNullOrWhiteSpace(dto.Username)
                                        || dto.Username.Length < 3
                                        || dto.Username.Length > 255)
            {
                throw new ArgumentException("Username must be between 3 and 255 characters long.", nameof(dto.Username));
            }

            if (string.IsNullOrWhiteSpace(dto.Password)
                                        || dto.Password.Length < 6
                                        || dto.Password.Length > 100)
            {
                throw new ArgumentException("Password must be between 6 and 100 characters long.", nameof(dto.Password));
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                throw new ArgumentException("Confirm Password must match Password.", nameof(dto.ConfirmPassword));
            }

            var existed = await _repository.GetAccountByUsnAsync(dto.Username);
            if (existed != null)
            {
                throw new InvalidOperationException($"Username '{dto.Username}' is already in use.");
            }

            var defaultRole = await _roleRepository.GetByIdAsync(7);
            if (defaultRole == null || defaultRole.RoleId <= 0)
            {
                throw new InvalidOperationException("Invalid role assignment.");
            }
            //      -- bổ sung phân biệt email nữa =Đ
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
            dto.Id = acc.AccountId;     //      cos theer xoas + xoas ben dto
            return dto;
        }


        public async Task<IEnumerable<RegisterDTO?>> GetALlAccountAsync()
        {
            var accounts = await _repository.GetAllAsync();
            return accounts.Select(acc => new RegisterDTO
            {
                Id = acc.AccountId,
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
                    Id = account.AccountId, // có thể dòng này del cần vì tính bảo mật =)))) nhưng mà em nghĩ có thể lơ được
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
            current.Password = dto.NewPassword;
            await _repository.UpdateAsync(current);
            return dto;
        }

        public async Task<ProfileDTO> UpdateProfile(ProfileDTO dto, string usn)
        {
            var current = await _repository.GetAccountByUsnAsync(usn);
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
                Id = current.AccountId,
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
            // if (acc.Password != HashPassword(pwd)) return false;
            if (acc.Password != pwd) return false;
            if (acc.AccountStatus == 0) return false;
            return true;
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
    }
}