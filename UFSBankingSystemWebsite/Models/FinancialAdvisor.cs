using System;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystem.Models
{
    public class FinancialAdvisor
    {
        [Key]
        public int FinancialAdvisorID { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string EmployeeNumber { get; set; }

        // Navigation property
        public virtual User User { get; set; }
        public virtual ICollection<FinancialAdvice> FinancialAdvices { get; set; }
    }
}
