using UFSBankingSystemWebsite.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UFSBankingSystemWebsite.Data.SeedData
{
    public static class SampleData
    {
        // Default Password for all users
        private const string DefaultPassword = "@TestApp123";

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
            new User { UserName = "thato@ufs.ac.za", FirstName = "Thato", LastName = "Moyo", Email ="thato@ufs.ac.za ", IDnumber=  "", StudentStaffNumber= "",UserRole=  "" }

        };

        public static List<User> SampleStaff { get; } = new List<User>
        {
            new User { UserName = "john@ufs.ac.za", FirstName = "John", LastName = "Doe", Email = "john@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "jane@ufs.ac.za", FirstName = "Jane", LastName = "Smith", Email = "jane@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "david@ufs.ac.za", FirstName = "David", LastName = "Johnson", Email = "david@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "sarah@ufs.ac.za", FirstName = "Sarah", LastName = "Williams", Email = "sarah@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" },
            new User { UserName = "michael@ufs.ac.za", FirstName = "Michael", LastName = "Brown", Email = "michael@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User" }
        };

        public static List<Transactions> SampleTransactions { get; } = new List<Transactions>
        {
            new Transactions { Amount=100m, TransactionDate=DateTime.Now.AddDays(-1), Reference="Initial Deposit for Thabo"},
            new Transactions { Amount=250m, TransactionDate=DateTime.Now.AddDays(-2), Reference="Initial Deposit for Sipho"},
            new Transactions { Amount=300m, TransactionDate=DateTime.Now.AddDays(-3), Reference="Initial Deposit for Nomsa"},
            new Transactions { Amount=150m, TransactionDate=DateTime.Now.AddDays(-4), Reference="Initial Deposit for Themba"},
            new Transactions { Amount=75m, TransactionDate=DateTime.Now.AddDays(-5), Reference="Initial Deposit for Lerato"},
            new Transactions { Amount=200m, TransactionDate=DateTime.Now.AddDays(-6), Reference="Initial Deposit for Thabiso"},
            new Transactions { Amount=400m, TransactionDate=DateTime.Now.AddDays(-7), Reference="Initial Deposit for Mpho"},
            new Transactions { Amount=120m, TransactionDate=DateTime.Now.AddDays(-8), Reference="Initial Deposit for Dineo"},
            new Transactions { Amount=180m, TransactionDate=DateTime.Now.AddDays(-9), Reference="Initial Deposit for Tshepiso"},
            new Transactions { Amount=220m, TransactionDate=DateTime.Now.AddDays(-10), Reference="Initial Deposit for Thato"}
        };

        public static List<Notification> SampleNotifications { get; } = new List<Notification>
        {
                new Notification { Message="New user registered: Thabo Zungu.", NotificationDate=DateTime.Now.AddMinutes(-10), IsRead=false },
                new Notification { Message="Transaction alert: $100 deposited to Thabo's account.", NotificationDate=DateTime.Now.AddMinutes(-5), IsRead=false },
                new Notification { Message = "Sipho has requested a password change.", NotificationDate = DateTime.Now.AddHours(-3), IsRead = false },
                new Notification { Message = "Nomsa has transferred funds between accounts.", NotificationDate = DateTime.Now.AddHours(-4), IsRead = false },
                new Notification { Message = "Themba has updated their profile information.", NotificationDate = DateTime.Now.AddHours(-5), IsRead = false },
                new Notification { Message = "Lerato has requested a balance inquiry.", NotificationDate = DateTime.Now.AddHours(-6), IsRead = false },
                new Notification { Message = "Thabiso has reported a suspicious transaction.", NotificationDate = DateTime.Now.AddHours(-7), IsRead = false },
                new Notification { Message = "Mpho has requested a statement of account.", NotificationDate = DateTime.Now.AddHours(-8), IsRead = false },
                new Notification { Message = "Dineo has received a transaction alert.", NotificationDate = DateTime.Now.AddHours(-9), IsRead = false },
                new Notification { Message = "Tshepiso has logged in successfully.", NotificationDate = DateTime.Now.AddHours(-10), IsRead = false }
        };

        //public static List<FeedBack> SampleFeedbacks { get; } = new List<FeedBack>
        //{
        //    new FeedBack
        //    {
        //        Id ="1a2b3c4d5e6f7g8h9i0j1k2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8b9c0d1e2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3a4b5c6d7e8f9g0h1i2j3k4l5m6n7o8p9q0r1s2t3u4v5w6x7y8z9a0b1c2d3e4f5g6h7i8j9k0l1m2n3o4p5q6r7s8t9u0v1w2x3y4z5a6b7c8d9e0f1g2h3i4j5k6l7m8n9o0p1q23456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
        //        Rating =  10,
        //        FeedbackDate= DateTime.Now,
        //        Message="Great service!",
        //        Id= User.Email // Assuming the Id is the user's email
        //    }
        //    // Add more feedback entries as needed...
        //};

        //public static List<FinancialAdvice> SampleFinancialAdvices { get; } = new List<FinancialAdvice>
        //{
        //     new FinancialAdvice
        //        {
        //            Id= user.Email,
        //            FinancialAdvisorID= 100,
        //            Advice="Consider diversifying your investments.",
        //            CreatedAt= DateTime.Now
        //        }
        //        // Add more financial advice entries as needed...
        //};

        public static async Task SeedDataAsync(UserManager<User> userManager)
        {
            foreach (var user in SampleCustomers)
            {
                var result = await userManager.CreateAsync(user, DefaultPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user {user.UserName}: {error.Description}");
                    }
                }
            }
        }
    }
}