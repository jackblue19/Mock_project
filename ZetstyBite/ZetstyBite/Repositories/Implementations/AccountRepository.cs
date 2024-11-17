using ZetstyBite.Data;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using ZetstyBite.Repositories.Interfaces;
using ZetstyBite.Models.Entities;


namespace ZetstyBite.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository, IRepository<Account>
    {
        private readonly ZestyBiteDbContext _context;
        public AccountRepository(ZestyBiteDbContext context)
        {
            _context = context;
        }
        /*      the logic inside shud be implemented for Service layer
         public async Task<Account> CreateAccountAsync(Account account)
        {
            var existed = await _context.Accounts
                                        .FirstOrDefaultAsync(acc => acc.Username == account.Username);
            if ( existed != null )
            {
                throw new InvalidOperationException($"Username '{account.Username}' already exists.");
            }
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            //return await Task.FromResult(account);
            return account;
        }
        */
        public async Task<Account> CreateAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task<Account?> GetAccountByUsnAsync(string usn)
        {
            return await _context.Accounts.FirstOrDefaultAsync(acc => acc.Username == usn);
        }

        public async Task<Account> AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        //  method below for fun -> dont use
        public async Task<Account> CreateAsync(Account entity)
        {
            await _context.Set<Account>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<bool> DeletedAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetByIdAsnyc(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> SearchAccountByNamesAsync(string names)
        {
            throw new NotImplementedException();
        }

        public Task<Account> UpdateAsync(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
