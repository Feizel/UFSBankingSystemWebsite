namespace UFSBankingSystem.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string UserId { get; set; } // Foreign key to User
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool IsRead { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}
