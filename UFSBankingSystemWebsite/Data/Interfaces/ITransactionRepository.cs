using UFSBankingSystemWebsite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface ITransactionRepository : IRepositoryBase<Transactions>
    {
        Task<List<Transactions>> GetTransactionsByUserIdAsync(string userId); // Get transactions for a specific user
        Task<decimal> GetTotalTransactionAmountAsync(string userId); // Calculate total transaction amount for a specific user
    }
}