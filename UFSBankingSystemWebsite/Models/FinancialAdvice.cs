using System;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystem.Models
{
    public class FinancialAdvice
    {
        [Key]
        public int FinancialAdviceID { get; set; }
        [Required]
        public int FinancialAdvisorID { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Advice { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual FinancialAdvisor FinancialAdvisor { get; set; }
        public virtual User User { get; set; }
    }
}