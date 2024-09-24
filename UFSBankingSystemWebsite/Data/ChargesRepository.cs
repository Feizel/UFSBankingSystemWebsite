using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;

namespace UFSBankingSystem.Data
{
    public class ChargesRepository : RepositoryBase<Charges>, IChargesRepository

    {
        public ChargesRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
