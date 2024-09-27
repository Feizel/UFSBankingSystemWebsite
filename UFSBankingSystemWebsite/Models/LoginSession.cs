using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFSBankingSystem.Models
{
    public class LoginSession
    {
        public int LoginSessionID { get; set; } // Primary Key
        public string UserId { get; internal set; } //Foreign Key
        public string UserEmail { get; set; }
        public DateTime TimeStamp { get; set; }

        // Navigation property (if needed)
        public virtual User User { get; set; }

    }
}