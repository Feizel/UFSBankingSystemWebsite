using System;
using System.Collections.Generic;

namespace UFSBankingSystem.Models
{
    public class Consultant
    {
        public int ConsultantID { get; set; } // Primary key
        public string Id { get; set; } // Foreign key to User

       public string FirstName { get; set; }
       public string LastName { get; set; }
        public string Email { get; set; } // Email for the consultant
        public string EmployeeNumber { get; set; } // Unique employee number for internal tracking

        // Navigation property to the associated user
        public virtual User User { get; set; }

        // A collection of financial advice given by this consultant (if applicable)
        public virtual ICollection<FinancialAdvice> FinancialAdvices { get; set; } = new List<FinancialAdvice>();
    }
}