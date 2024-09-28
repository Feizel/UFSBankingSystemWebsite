using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystem.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        [Required]
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime NotificationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}
