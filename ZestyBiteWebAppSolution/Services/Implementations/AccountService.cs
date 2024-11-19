using Microsoft.AspNetCore.Identity;
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

            // Validate Username
            if (string.IsNullOrWhiteSpace(dto.Username) || dto.Username.Length < 3 || dto.Username.Length > 255) {
                throw new ArgumentException("Username must be between 3 and 255 characters long.", nameof(dto.Username));
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6 || dto.Password.Length > 100) {
                throw new ArgumentException("Password must be between 6 and 100 characters long.", nameof(dto.Password));
            }

            // Validate Confirm Password
            if (dto.Password != dto.ConfirmPassword) {
                throw new ArgumentException("Confirm Password must match Password.", nameof(dto.ConfirmPassword));
            }

            // Check if username already exists
            var existed = await _repository.GetAccountByUsnAsync(dto.Username);
            if (existed != null) {
                throw new InvalidOperationException($"Username '{dto.Username}' is already in use.");
            }

            // Create new account
            var acc = new Account() {
                UserName = dto.Username,
                Password = HashPassword(dto.Password), // Hash the password
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Gender = dto.Gender,
                Email = dto.Email,
                ProfileImage = dto.ProfileImg,
                RoleId = 7,
                VerificationCode = "Samplecode"
            };

            // Add the new account to the repository
            var created = await _repository.AddAsync(acc);

            return dto;
        }

        Task<Account> IAccountService.GetAccountById(int id) {
            throw new NotImplementedException();
        }
        private string HashPassword(string password) {
            var passwordHasher = new PasswordHasher<object>(); // You can use any object here, e.g., your user model
            return passwordHasher.HashPassword("", password); // Pass null for the user parameter
        }
    }
}