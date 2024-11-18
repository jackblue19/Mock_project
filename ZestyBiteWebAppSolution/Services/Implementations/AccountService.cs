using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations {
    public class AccountService : IAccountService {
        private readonly IAccountRepository _repository;
        public AccountService(IAccountRepository accountRepository) {
            _repository = accountRepository;
        }
        public async Task<Account> CreateAccountAsync(Account account) {
            if (account == null) {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            }

            var existed = await _repository.GetAccountByUsnAsync(account.UserName);
            if (existed != null) {
                throw new InvalidOperationException($"Username '{account.UserName}' is already in use.");
            }

            var created = await _repository.CreateAccountAsync(account);
            return created;
        }

        public Task<IEnumerable<Account>> GetALlAccountAsync() {
            throw new NotImplementedException();
        }

        public async Task<AccountDTO> SignUpAsync(AccountDTO dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto), "Input account was null.");
            }

            var existed = await _repository.GetAccountByUsnAsync(dto.Username);
            if (existed != null) {
                throw new InvalidOperationException($"Username '{dto.Username}' is already in use.");
            }

            var acc = new Account() {
                AccountId = dto.Id,
                UserName = dto.Username,
                Password = dto.Password,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Gender = dto.Gender, // gpt rcm =D
                Email = dto.Email,
                ProfileImage = dto.ProfileImg,
                RoleId = 7,
                // VerificationCode = Guid.NewGuid().ToString() ,
                VerificationCode = "Samplecode"

            };

            var created = await _repository.AddAsync(acc);
            dto.Id = created.AccountId;
            return dto;
        }

        Task<Account> IAccountService.GetAccountById(int id) {
            throw new NotImplementedException();
        }
    }
}