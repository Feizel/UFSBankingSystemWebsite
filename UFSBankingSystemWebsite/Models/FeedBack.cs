using System;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models
{
    public class FeedBack
    {
        [Key]
        public int FeedBackID { get; set; }
        [Required]
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        public DateTime FeedbackDate { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}