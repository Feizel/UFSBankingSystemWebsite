using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;

namespace UFSBankingSystem.Data
{
  
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository

    {
        public TransactionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
