using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UFSBankingSystemWebsite.Models
{
    public class Consultant
    {
        [Key]
        public int ConsultantID { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string EmployeeNumber { get; set; }

        // Navigation property
        public virtual User User { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}