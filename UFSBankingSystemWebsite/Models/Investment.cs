using System;

namespace UFSBankingSystem.Models
{
    public class Investment
    {
        public int InvestmentID { get; set; } // Primary key
        public string UserId { get; set; } // Foreign key to User
        public decimal Value { get; set; } // Current value of the investment
        public decimal InitialValue { get; set; } // Initial investment amount
        public DateTime InvestmentDate { get; set; } // Date of the investment
        public string InvestmentType { get; set; } // Type of investment (e.g., Stocks, Bonds, Real Estate)
        public string Description { get; set; } // Description of the investment

        // Navigation property
        public virtual User User { get; set; }
    }
}