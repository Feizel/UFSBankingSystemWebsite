using UFSBankingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystem.Data
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetNotificationsByUserEmailAsync(string email)
        {
            return await _context.Notifications.Where(n => n.UserEmail == email).ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true; // Assuming IsRead is a property on Notification
                await UpdateAsync(notification);
            }
        }

        public async Task<List<Notification>> GetAllUnreadNotificationsAsync()
        {
            return await _context.Notifications.Where(n => !n.IsRead).ToListAsync(); // Assuming IsRead is a property on Notification
        }
    }
}