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

        public AccountService(IAccountRepository accountRepository , IRoleRepository roleRepository)
        {
            _repository = accountRepository;
            _roleRepository = roleRepository;
        }

        //  async Task<Account> IAccountService.CreateAccountAsync(Account account)
        // -> still work <=> no 'public' keyword

        //  Fix the logic inside => no verification code needed and might use and return DTO instead
        public async Task<Account> CreateStaffAsync(Account account , int roleId)
        {
            if ( account == null )
            {
                throw new ArgumentNullException(nameof(account) , "Account cannot be null");
            }

            var existed = await _repository.GetAccountByUsnAsync(account.UserName);
            if ( existed != null )
            {
                throw new InvalidOperationException($"Username '{account.UserName}' is already in use.");
            }

            var created = await _repository.CreateAsync(account);
            return created;
        }

        public async Task<AccountDTO> SignUpAsync(AccountDTO dto)
        {
            if ( dto == null )
            {
                throw new ArgumentNullException(nameof(dto) , "Input account was null.");
            }

            if ( string.IsNullOrWhiteSpace(dto.Username)
                                        || dto.Username.Length < 3
                                        || dto.Username.Length > 255 )
            {
                throw new ArgumentException("Username must be between 3 and 255 characters long." , nameof(dto.Username));
            }

            if ( string.IsNullOrWhiteSpace(dto.Password)
                                        || dto.Password.Length < 6
                                        || dto.Password.Length > 100 )
            {
                throw new ArgumentException("Password must be between 6 and 100 characters long." , nameof(dto.Password));
            }

            if ( dto.Password != dto.ConfirmPassword )
            {
                throw new ArgumentException("Confirm Password must match Password." , nameof(dto.ConfirmPassword));
            }

            var existed = await _repository.GetAccountByUsnAsync(dto.Username);
            if ( existed != null )
            {
                throw new InvalidOperationException($"Username '{dto.Username}' is already in use.");
                throw new ArgumentException("Please choose another username!" , nameof(dto.Username));
            }

            existed = await _repository.GetAccountByEmailAsync(dto.Email);
            if ( existed != null )
            {
                throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");
                throw new ArgumentException("Please choose another E-mail!" , nameof(dto.Email));
            }

            var defaultRole = await _roleRepository.GetByIdAsync(7);
            var acc = new Account()
            {
                UserName = dto.Username ,
                Password = HashPassword(dto.Password) ,
                Name = dto.Name ,
                PhoneNumber = dto.PhoneNumber ,
                Address = dto.Address ,
                Gender = dto.Gender ,
                Email = dto.Email ,
                ProfileImage = dto.ProfileImg ,
                VerificationCode = dto.VerificationCode
            };
            //acc.Role = defaultRole;
            acc.RoleId = defaultRole.RoleId;

            var created = await _repository.CreateAsync(acc);
            dto.Id = acc.AccountId;
            return dto;
        }
        public async Task<IEnumerable<Account?>> GetALlAccountAsync()
        {
            var accounts = await _repository.GetAllAsync();
            return accounts;
        }
        public async Task<Account?> GetAccountById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /* Other method */
        private string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>(); // You can use any object here, e.g., your user model
            return passwordHasher.HashPassword("" , password); // Pass null for the user parameter
        }

    }
}