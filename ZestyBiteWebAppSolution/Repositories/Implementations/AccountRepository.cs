using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;


namespace ZestyBiteWebAppSolution.Repositories.Implementations
{
    public class AccountRepository :IAccountRepository, IRepository<Account>
    {
        private readonly ZestyBiteContext _context;
        public AccountRepository(ZestyBiteContext context)
        {
            _context = context;
        }
                                    /* Interface */
        public async Task<IEnumerable<Account?>> SearchAccountByNamesAsync(string name)
        {
            return await _context.Accounts
                                 .Where(acc => acc.Name == name)
                                 .ToListAsync();
        }
        public async Task<Account> CreateAccountAsync(Account account , sbyte roleId)
        {
            account.RoleId = roleId;
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task<Account?> GetAccountByUsnAsync(string usn)
        {
            return await _context.Accounts
                                 .Include(acc => acc.Role)
                                 .FirstOrDefaultAsync(acc => acc.Username == usn);
        }
        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts
                                 .FirstOrDefaultAsync(acc => acc.Email == email);
        }

                                    /* Generic */
        public async Task<IEnumerable<Account?>> GetAllAsync()
        {
            return await _context.Accounts
                                 .Include(acc => acc.Role)
                                 .ToListAsync();
        }
        public async Task<Account?> GetByIdAsync(int id)
        {
            // return await _context.Accounts
            //                      .Where(acc => acc.AccountId == id)
            //                      .SingleOrDefaultAsync();
            return await _context.Accounts
                                 .Include(acc => acc.Role)
                                 .FirstOrDefaultAsync(acc => acc.AccountId == id);
        }
        public async Task<Account> CreateAsync(Account acc)
        {
            // await _context.Set<Account>().AddAsync(entity);  // => not sure to test
            // await _context.Accounts.AddAsync(entity); // => not rcm to use
            _context.Accounts.Add(acc);
            await _context.SaveChangesAsync();
            return acc;
        }
        public async Task<Account> UpdateAsync(Account entity)
        {
            _context.Accounts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<Account> DeleteAsync(Account entity)
        {
            _context.Accounts.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}