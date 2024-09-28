using UFSBankingSystem.Models;
using System.Collections.Generic;

namespace UFSBankingSystem.Data.SeedData
{
    public static class SampleData
    {
        public static List<User> SampleCustomers { get; } = new List<User>
        {
            new User { UserName = "thabo@ufs.ac.za", FirstName = "Thabo", LastName = "Zungu", Email = "thabo@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "sipho@ufs.ac.za", FirstName = "Sipho", LastName = "Ndlovu", Email = "sipho@ufs.ac.za", IDnumber = "9312251234567", StudentStaffNumber = "1234567890", UserRole = "User" },
            new User { UserName = "nomsa@ufs.ac.za", FirstName = "Nomsa", LastName = "Mkhize", Email = "nomsa@ufs.ac.za", IDnumber = "8506302345678", StudentStaffNumber = "0987654321", UserRole = "User" },
            new User { UserName = "themba@ufs.ac.za", FirstName = "Themba", LastName = "Dlamini", Email = "themba@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "lerato@ufs.ac.za", FirstName = "Lerato", LastName = "Mokoena", Email = "lerato@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "thabiso@ufs.ac.za", FirstName = "Thabiso", LastName = "Khumalo", Email = "thabiso@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "mpho@ufs.ac.za", FirstName = "Mpho", LastName = "Mabuza", Email = "mpho@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "dineo@ufs.ac.za", FirstName = "Dineo", LastName = "Nkosi", Email = "dineo@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "tshepiso@ufs.ac.za", FirstName = "Tshepiso", LastName = "Maseko", Email = "tshepiso@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "thato@ufs.ac.za", FirstName = "Thato", LastName = "Moyo", Email = "thato@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" }
        };

        public static List<User> SampleStaff { get; } = new List<User>
        {
            new User { UserName = "john@ufs.ac.za", FirstName = "John", LastName = "Doe", Email = "john@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "jane@ufs.ac.za", FirstName = "Jane", LastName = "Smith", Email = "jane@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "david@ufs.ac.za", FirstName = "David", LastName = "Johnson", Email = "david@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "sarah@ufs.ac.za", FirstName = "Sarah", LastName = "Williams", Email = "sarah@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "michael@ufs.ac.za", FirstName = "Michael", LastName = "Brown", Email = "michael@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" }
        };

        public static List<Transaction> SampleTransactions { get; } = new List<Transaction>
        {
            new Transaction { UserEmail = "thabo@ufs.ac.za", Amount = 100m, TransactionDate = DateTime.Now.AddDays(-1) },
            new Transaction { UserEmail = "sipho@ufs.ac.za", Amount = 250m, TransactionDate = DateTime.Now.AddDays(-2) },
            new Transaction { UserEmail = "nomsa@ufs.ac.za", Amount = 300m, TransactionDate = DateTime.Now.AddDays(-3) },
            new Transaction { UserEmail = "themba@ufs.ac.za", Amount = 150m, TransactionDate = DateTime.Now.AddDays(-4) },
            new Transaction { UserEmail = "lerato@ufs.ac.za", Amount = 75m, TransactionDate = DateTime.Now.AddDays(-5) },
            new Transaction { UserEmail = "thabiso@ufs.ac.za", Amount = 200m, TransactionDate = DateTime.Now.AddDays(-6) },
            new Transaction { UserEmail = "mpho@ufs.ac.za", Amount = 400m, TransactionDate = DateTime.Now.AddDays(-7) },
            new Transaction { UserEmail = "dineo@ufs.ac.za", Amount = 120m, TransactionDate = DateTime.Now.AddDays(-8) },
            new Transaction { UserEmail = "tshepiso@ufs.ac.za", Amount = 180m, TransactionDate = DateTime.Now.AddDays(-9) },
            new Transaction { UserEmail = "thato@ufs.ac.za", Amount = 220m, TransactionDate = DateTime.Now.AddDays(-10) }
        };

        public static List<Notification> SampleNotifications { get; } = new List<Notification>
        {
            new Notification { Message = "New user registered: Thabo", NotificationDate = DateTime.Now.AddMinutes(-10), IsRead = false },
            new Notification { Message = "Transaction alert: $100 deposited to user account.", NotificationDate = DateTime.Now.AddMinutes(-5), IsRead = false },
            new Notification { Message = "Consultant Thando has submitted a report.", NotificationDate = DateTime.Now.AddHours(-1), IsRead = false },
            new Notification { Message = "Financial Advisor Millicent has provided advice.", NotificationDate = DateTime.Now.AddHours(-2), IsRead = false },
            new Notification { Message = "Sipho has requested a password change.", NotificationDate = DateTime.Now.AddHours(-3), IsRead = false },
            new Notification { Message = "Nomsa has transferred funds between accounts.", NotificationDate = DateTime.Now.AddHours(-4), IsRead = false },
            new Notification { Message = "Themba has updated their profile information.", NotificationDate = DateTime.Now.AddHours(-5), IsRead = false },
            new Notification { Message = "Lerato has requested a balance inquiry.", NotificationDate = DateTime.Now.AddHours(-6), IsRead = false },
            new Notification { Message = "Thabiso has reported a suspicious transaction.", NotificationDate = DateTime.Now.AddHours(-7), IsRead = false },
            new Notification { Message = "Mpho has requested a statement of account.", NotificationDate = DateTime.Now.AddHours(-8), IsRead = false }
        };
    }
}