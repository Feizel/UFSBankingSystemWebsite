using System;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models
{
    public class FinancialAdvisor
    {
        [Key]
        public int FinancialAdvisorID { get; set; }
        [Required]
        public string Id { get; set; } // Foreign key to User
        [Required]
        [StringLength(20)]
        public string EmployeeNumber { get; set; }

        // Navigation property
        public virtual User User { get; set; }
        public virtual ICollection<FinancialAdvice> FinancialAdvices { get; set; }
    }
}
