using System;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models
{
    public class FinancialAdvice
    {
        [Key]
        public int FinancialAdviceID { get; set; }
        [Required]
        public int FinancialAdvisorID { get; set; }
        [Required]
        public string Id { get; set; } // Foreign key to User
        [Required]
        public string Advice { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual FinancialAdvisor FinancialAdvisor { get; set; }
        public virtual User User { get; set; }
    }
}