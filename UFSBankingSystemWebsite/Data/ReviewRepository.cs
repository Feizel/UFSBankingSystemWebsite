using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;

namespace UFSBankingSystem.Data
{
    public class ReviewRepository : RepositoryBase<FeedBack>, IReviewRepository
    {
        public ReviewRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
