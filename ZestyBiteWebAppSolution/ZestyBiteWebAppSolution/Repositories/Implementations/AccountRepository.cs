using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;


namespace ZestyBiteWebAppSolution.Repositories.Implementations {
    public class AccountRepository : IAccountRepository {
        private readonly ZestybiteContext _context;
        public AccountRepository(ZestybiteContext context) {
            _context = context;
        }
        public async Task<Account> CreateAccountAsync(Account account) {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task<Account?> GetAccountByUsnAsync(string usn) {
            return await _context.Accounts.FirstOrDefaultAsync(acc => acc.UserName == usn);
        }

        public async Task<Account> AddAsync(Account account) {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        //  method below for fun -> dont use
        public async Task<Account> CreateAsync(Account entity) {
            await _context.Set<Account>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<bool> DeletedAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAllAsync() {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetByIdAsnyc(int id) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> SearchAccountByNamesAsync(string names) {
            throw new NotImplementedException();
        }

        public Task<Account> UpdateAsync(Account entity) {
            throw new NotImplementedException();
        }
    }
}