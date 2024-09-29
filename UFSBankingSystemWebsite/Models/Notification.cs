using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        [Required]
        public string Id { get; set; } // Foreign key to the User
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
