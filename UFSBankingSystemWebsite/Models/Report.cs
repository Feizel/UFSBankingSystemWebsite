using System;

namespace UFSBankingSystem.Models
{
    public class Report
    {
        public int ReportID { get; set; } // Primary key for the report
        public string UserId { get; set; } // Foreign key to User table
        public string Content { get; set; } // Content of the report
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Timestamp when the report was created

        // Navigation property for related user (if needed)
        public virtual User User { get; set; }
    }
}