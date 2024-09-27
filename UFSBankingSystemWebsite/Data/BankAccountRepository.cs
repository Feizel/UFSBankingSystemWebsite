using UFSBankingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystem.Data
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        private readonly AppDbContext _context;

        public BankAccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync()
        {
            return await _context.BankAccounts.Include(b => b.User).ToListAsync(); // Include user details if needed
        }

        public async Task<BankAccount> GetAccountByIdAsync(int id)
        {
            return await _context.BankAccounts.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id.ToString());
        }

        public async Task CreateAccountAsync(BankAccount account)
        {
            await _context.BankAccounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(BankAccount account)
        {
            _context.BankAccounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await GetAccountByIdAsync(id);
            if (account != null)
            {
                _context.BankAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}