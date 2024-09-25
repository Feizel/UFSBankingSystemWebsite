using UFSBankingSystem.Models;

namespace UFSBankingSystem.Models
{
    public class Consultant
    {
        public int ConsultantID { get; set; } // Primary key
        public string UserId { get; set; } // Foreign key to User

        // Navigation property to the associated user
        public virtual User User { get; set; }
    }
}