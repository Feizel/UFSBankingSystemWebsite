using System;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models
{
    public class Report
    {
        [Key]
        public int ReportID { get; set; }
        [Required]
        public int ConsultantID { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual Consultant Consultant { get; set; }
    }
}