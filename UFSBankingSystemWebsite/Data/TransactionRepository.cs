using UFSBankingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystem.Data
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(string userId)
        {
            return await _context.Transactions.Where(t => t.UserEmail == userId).ToListAsync();
        }

        public async Task<decimal> GetTotalTransactionAmountAsync(string userId)
        {
            return await _context.Transactions
                .Where(t => t.UserEmail == userId)
                .SumAsync(t => t.Amount);
        }
    }
}