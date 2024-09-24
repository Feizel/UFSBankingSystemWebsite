using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;

namespace UFSBankingSystem.Data
{
  
    public class TransactionRepository : RepositoryBase<Transactions>, ITransactionRepository

    {
        public TransactionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
