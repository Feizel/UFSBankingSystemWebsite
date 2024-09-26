using UFSBankingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        Task<List<Transaction>> GetTransactionsByUserIdAsync(string userId); // Get transactions for a specific user
        Task<decimal> GetTotalTransactionAmountAsync(string userId); // Calculate total transaction amount for a specific user
    }
}