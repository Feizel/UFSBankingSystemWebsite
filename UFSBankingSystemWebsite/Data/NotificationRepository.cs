using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;

namespace UFSBankingSystem.Data
{
  
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {

        public NotificationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
