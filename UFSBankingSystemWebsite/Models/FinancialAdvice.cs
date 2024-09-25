using System;

namespace UFS_Banking_System_Website.Models
{
    public class FinancialAdvice
    {
        public int FinancialAdviceID { get; set; } // Primary key
        public string UserId { get; set; } // Foreign key to User
        public string Advice { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}