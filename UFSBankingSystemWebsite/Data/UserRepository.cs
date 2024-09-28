using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystem.Data
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserViewModel>> GetAllUsersAndBankAccountAsync()
        {
            return await _context.Users
                .Include(u => u.BankAccounts) 
                .Select(u => new UserViewModel
                {
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    AccountNumber = u.BankAccounts.Select(b => b.AccountNumber).FirstOrDefault(), // Get first account number
                    Balance = u.BankAccounts.Sum(b => b.Balance) // Total balance across all accounts
                })
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}