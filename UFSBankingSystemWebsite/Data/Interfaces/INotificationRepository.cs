using UFSBankingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UFSBankingSystemWebsite.Data.Interfaces
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        Task<List<Notification>> GetNotificationsByUserEmailAsync(string email); // Retrieve notifications for a specific user by email
        Task MarkAsReadAsync(int notificationId); // Mark a specific notification as read
        Task<List<Notification>> GetAllUnreadNotificationsAsync(); // Retrieve all unread notifications
    }
}