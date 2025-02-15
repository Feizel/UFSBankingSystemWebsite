using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UFSBankingSystemWebsite.Models
{
    public class LoginSession
    {
        [Key]
        public int LoginSessionID { get; set; }
        [Required]
        public string Id { get; set; } // Foreign key to User
        public string UserEmail { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }

        // Navigation property
        public virtual User User { get; set; }

    }
}