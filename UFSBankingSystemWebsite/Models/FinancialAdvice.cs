using System;

namespace UFSBankingSystem.Models
{
    public class FinancialAdvice
    {
        public int FinancialAdviceID { get; set; } // Primary key
        public string Id { get; set; } // Foreign key to User
        public string Advice { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}