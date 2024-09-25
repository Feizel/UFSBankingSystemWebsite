using Microsoft.AspNetCore.Identity;
using UFS_Banking_System_Website.Models;

namespace UFSBankingSystem.Models
{
    public class User : IdentityUser
    {
        public int ConsultantID { get; set; } // foreign key
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long StudentStaffNumber { get; set; }
        public long IDnumber { get; set; }
        public string AccountNumber { get; set; }
        public string UserRole { get; set; }

        // Navigation properties
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<FinancialAdvice> FinancialAdvices { get; set; } = new List<FinancialAdvice>();
        public bool IsConsultant { get; set; }
        public bool IsCustomer { get; set; }
    }
}
