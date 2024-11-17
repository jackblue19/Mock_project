using Microsoft.EntityFrameworkCore;
using ZetstyBite.Models.Entities;
using ZetstyBite.Repositories.Interfaces;
using ZetstyBite.Services.Interfaces;
using ZetstyBite.Models.DTOs;
using Humanizer;

namespace ZetstyBite.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        public AccountService() { }
        public AccountService(IAccountRepository accountRepository)
        {
            _repository = accountRepository;
        }

        //  async Task<Account> IAccountService.CreateAccountAsync(Account account)
        // -> still work <=> no 'public' keyword

        public async Task<Account> CreateAccountAsync(Account account)
        {
            if ( account == null )
            {
                throw new ArgumentNullException(nameof(account) , "Account cannot be null");
            }

            var existed = await _repository.GetAccountByUsnAsync(account.Username);
            if ( existed != null )
            {
                throw new InvalidOperationException($"Username '{account.Username}' is already in use.");
            }

            var created = await _repository.CreateAccountAsync(account);
            return created;
        }

        public async Task<AccountDTO> SignUpAsync(AccountDTO dto)
        {
            if ( dto == null )
            {
                throw new ArgumentNullException(nameof(dto) , "Input account was null.");
            }

            var existed = await _repository.GetAccountByUsnAsync(dto.Username);
            if ( existed != null )
            {
                throw new InvalidOperationException($"Username '{dto.Username}' is already in use.");
            }

            var acc = new Account()
            {
                AccountId = dto.Id,
                Username = dto.Username ,
                Password = dto.Password ,
                Name = dto.FullName ,
                PhoneNumber = dto.Phone ,
                Address = dto.Address ,
                Gender = dto.Gender ? 1UL : 0UL , // gpt rcm =D
                Email = dto.Email ,
                ProfileImage = dto.ProfileImg ,
                RoleId = 7 ,
                // VerificationCode = Guid.NewGuid().ToString() ,
                VerificationCode = "Samplecode"

            };

            var created = await _repository.AddAsync( acc );

            // return new AccountDTO
            // {
            //     Id = created.AccountId ,
            //     Username = created.Username ,
            //     Password = created.Password ,   
            //     FullName = created.Name ,
            //     Phone = created.PhoneNumber ,
            //     Gender = created.Gender == 1,
            //     Email = created.Email ,
            //     ProfileImg = created.ProfileImage ,
            //     Address = created.Address ,
            //     RoleDescription = created.Role.RoleDescription ,
            // };
            dto.Id = created.AccountId;
            return dto;
        }

        async Task<IEnumerable<Account>> IAccountService.GetALlAccountAsync()
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
