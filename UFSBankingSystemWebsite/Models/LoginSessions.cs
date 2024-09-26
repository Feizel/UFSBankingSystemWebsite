using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFSBankingSystem.Models
{
    public class LoginSessions
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserId { get; internal set; }
    }
}