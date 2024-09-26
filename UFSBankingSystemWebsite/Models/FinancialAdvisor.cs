using System;

namespace UFSBankingSystem.Models
{
    public class FinancialAdvisor
    {
        public int Id { get; set; } // Unique identifier for the financial advisor
        public string UserName { get; set; } // Username for login purposes
        public string FirstName { get; set; } // First name of the financial advisor
        public string LastName { get; set; } // Last name of the financial advisor
        public string Email { get; set; } // Email address for communication
        public string IDnumber { get; set; } // South African ID number
        public string PhoneNumber { get; set; } // Contact number
        public DateTime DateOfBirth { get; set; } // Date of birth for identification purposes
        public string EmployeeNumber { get; set; } // Unique employee number for internal tracking

        // Navigation properties (if needed)
        public virtual ICollection<FinancialAdvice> FinancialAdvices { get; set; } // List of advice given by this advisor
    }
}
