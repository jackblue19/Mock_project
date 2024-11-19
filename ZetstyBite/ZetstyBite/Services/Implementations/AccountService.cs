using Microsoft.EntityFrameworkCore;
using ZetstyBite.Models.Entities;
using ZetstyBite.Repositories.Interfaces;
using ZetstyBite.Services.Interfaces;
using Humanizer;

namespace ZetstyBite.Services.Implementations
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

        //  async Task<Account> IAccountService.CreateAccountAsync(Account account)
        // -> still work <=> no 'public' keyword

        public async Task<Account> CreateAccountAsync(Account account)
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

        public async Task<Account> SignUpAsync(Account dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Input account was null.");
            }

            var existed = await _repository.GetAccountByUsnAsync(dto.Username);
            if (existed != null)
            {
                throw new InvalidOperationException($"Username '{dto.Username}' is already in use.");
            }

            existed = await _repository.GetAccountByEmailAsync(dto.Email);
            if (existed != null)
            {
                throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");
            }

            var role = await _roleRepository.GetById(7);
            dto.Role = role;
            dto.RoleId = 7;
            var acc = new Account
            {
                AccountId = dto.AccountId,
                Username = dto.Username,
                Name = dto.Name,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Gender = dto.Gender,
                Email = dto.Email,
                Role = dto.Role,
                ProfileImage = dto.ProfileImage,
                VerificationCode = dto.VerificationCode,
            };
            // acc.Role = role;
            var created = await _repository.CreateAsync(acc);
            return created;
        }

        async Task<IEnumerable<Account?>> IAccountService.GetALlAccountAsync()
        {
            var accounts = await _repository.GetAllAsync();
            return accounts;
        }

        Task<Account> IAccountService.GetAccountById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
