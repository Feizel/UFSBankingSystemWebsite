using UFSBankingSystemWebsite.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UFSBankingSystemWebsite.Data.SeedData
{
    public static class SampleData
    {
        // Default Password for all users
        public const string DefaultPassword = "@TestApp123";

        private static readonly Random random = new Random();
        public static List<User> SampleCustomers { get; } = new List<User>
        {
            new User { UserName = "thabo@ufs.ac.za", FirstName = "Thabo", LastName = "Zungu", Email = "thabo@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true},
            new User { UserName = "thabo@ufs.ac.za", FirstName = "Thabo", LastName = "Zungu", Email = "thabo@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "sipho@ufs.ac.za", FirstName = "Sipho", LastName = "Ndlovu", Email = "sipho@ufs.ac.za", IDnumber = "9312251234567", StudentStaffNumber = "1234567890", UserRole = "User", IsCustomer = true },
            new User { UserName = "nomsa@ufs.ac.za", FirstName = "Nomsa", LastName = "Mkhize", Email = "nomsa@ufs.ac.za", IDnumber = "8506302345678", StudentStaffNumber = "0987654321", UserRole = "User", IsCustomer = true },
            new User { UserName = "themba@ufs.ac.za", FirstName = "Themba", LastName = "Dlamini", Email = "themba@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "lerato@ufs.ac.za", FirstName = "Lerato", LastName = "Mokoena", Email = "lerato@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "thabiso@ufs.ac.za", FirstName = "Thabiso", LastName = "Khumalo", Email = "thabiso@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "mpho@ufs.ac.za", FirstName = "Mpho", LastName = "Mabuza", Email = "mpho@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "dineo@ufs.ac.za", FirstName = "Dineo", LastName = "Nkosi", Email = "dineo@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "tshepiso@ufs.ac.za", FirstName = "Tshepiso", LastName = "Maseko", Email = "tshepiso@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "thato@ufs.ac.za", FirstName = "Thato", LastName = "Moyo", Email ="thato@ufs.ac.za ", IDnumber = "0109171810183", StudentStaffNumber= "4832108912",UserRole= "User", IsCustomer = true }

        };

        public static List<User> SampleStaff { get; } = new List<User>
        {
            new User { UserName = "john@ufs.ac.za", FirstName = "John", LastName = "Doe", Email = "john@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "jane@ufs.ac.za", FirstName = "Jane", LastName = "Smith", Email = "jane@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "david@ufs.ac.za", FirstName = "David", LastName = "Johnson", Email = "david@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "sarah@ufs.ac.za", FirstName = "Sarah", LastName = "Williams", Email = "sarah@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true },
            new User { UserName = "michael@ufs.ac.za", FirstName = "Michael", LastName = "Brown", Email = "michael@ufs.ac.za", IDnumber = "0206151810182", StudentStaffNumber = "7432108965", UserRole = "User", IsCustomer = true }
        };

        public static List<Transactions> SampleTransactions { get; } = new List<Transactions>
        {
            new Transactions { Amount=100m, TransactionDate=DateTime.Now.AddDays(-1), Reference="Initial Deposit for Thabo", BankAccountID=1, BankAccountIdSender=1, BankAccountIdReceiver=1, TransactionType=TransactionType.Deposit, BalanceAfter=100m },
            new Transactions { Amount=250m, TransactionDate=DateTime.Now.AddDays(-2), Reference="Initial Deposit for Sipho", BankAccountID=2, BankAccountIdSender=2, BankAccountIdReceiver=2, TransactionType=TransactionType.Deposit, BalanceAfter=250m },
            new Transactions { Amount=300m, TransactionDate=DateTime.Now.AddDays(-3), Reference="Initial Deposit for Nomsa", BankAccountID=3, BankAccountIdSender=3, BankAccountIdReceiver=3, TransactionType=TransactionType.Deposit, BalanceAfter=300m },
            new Transactions { Amount=150m, TransactionDate=DateTime.Now.AddDays(-4), Reference="Initial Deposit for Themba", BankAccountID=4, BankAccountIdSender=4, BankAccountIdReceiver=4, TransactionType=TransactionType.Deposit, BalanceAfter=150m },
            new Transactions { Amount=75m, TransactionDate=DateTime.Now.AddDays(-5), Reference="Initial Deposit for Lerato", BankAccountID=5, BankAccountIdSender=5, BankAccountIdReceiver=5, TransactionType=TransactionType.Deposit, BalanceAfter=75m },
            new Transactions { Amount=200m, TransactionDate=DateTime.Now.AddDays(-6), Reference="Initial Deposit for Thabiso", BankAccountID=6, BankAccountIdSender=6, BankAccountIdReceiver=6, TransactionType=TransactionType.Deposit, BalanceAfter=200m },
            new Transactions { Amount=400m, TransactionDate=DateTime.Now.AddDays(-7), Reference="Initial Deposit for Mpho", BankAccountID=7, BankAccountIdSender=7, BankAccountIdReceiver=7, TransactionType=TransactionType.Deposit, BalanceAfter=400m },
            new Transactions { Amount=120m, TransactionDate=DateTime.Now.AddDays(-8), Reference="Initial Deposit for Dineo", BankAccountID=8, BankAccountIdSender=8, BankAccountIdReceiver=8, TransactionType=TransactionType.Deposit, BalanceAfter=120m },
            new Transactions { Amount=180m, TransactionDate=DateTime.Now.AddDays(-9), Reference="Initial Deposit for Tshepiso", BankAccountID=9, BankAccountIdSender=9, BankAccountIdReceiver=9, TransactionType=TransactionType.Deposit, BalanceAfter=180m },
            new Transactions { Amount=220m, TransactionDate=DateTime.Now.AddDays(-10), Reference="Initial Deposit for Thato", BankAccountID=10, BankAccountIdSender=10, BankAccountIdReceiver=10, TransactionType=TransactionType.Deposit, BalanceAfter=220m }
        };

        public static List<Notification> SampleNotifications { get; } = new List<Notification>
        {
            new Notification { Id = "", UserEmail = "thabo@ufs.ac.za", Message = "Welcome to the banking system!", NotificationDate = DateTime.Now.AddMinutes(-10), IsRead = false },
            new Notification { Id = "", UserEmail = "sipho@ufs.ac.za", Message = "Your account has been successfully created.", NotificationDate = DateTime.Now.AddMinutes(-20), IsRead = false },
            new Notification { Id = "", UserEmail = "nomsa@ufs.ac.za", Message = "Your account balance is low.", NotificationDate = DateTime.Now.AddMinutes(-30), IsRead = false },
            new Notification { Id = "", UserEmail = "themba@ufs.ac.za", Message = "New features have been added to your account.", NotificationDate = DateTime.Now.AddMinutes(-40), IsRead = false },
            new Notification { Id = "", UserEmail = "lerato@ufs.ac.za", Message = "Your recent transaction was successful.", NotificationDate = DateTime.Now.AddMinutes(-50), IsRead = false },
            new Notification { Id = "", UserEmail = "thabiso@ufs.ac.za", Message = "Your password has been changed successfully.", NotificationDate = DateTime.Now.AddMinutes(-60), IsRead = false },
            new Notification { Id = "", UserEmail = "mpho@ufs.ac.za", Message = "You have a new message from support.", NotificationDate = DateTime.Now.AddMinutes(-70), IsRead = false },
            new Notification { Id = "", UserEmail = "dineo@ufs.ac.za", Message = "Your profile has been updated.", NotificationDate = DateTime.Now.AddMinutes(-80), IsRead = false },
            new Notification { Id = "", UserEmail = "tshepiso@ufs.ac.za", Message = "Your account settings have been changed.", NotificationDate = DateTime.Now.AddMinutes(-90), IsRead = false },
            new Notification { Id = "", UserEmail = "thato@ufs.ac.za", Message = "Thank you for using our banking services!", NotificationDate = DateTime.Now.AddMinutes(-100), IsRead = false }
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

        public static decimal GetRandomBalance()
        {
            return Math.Round((decimal)(random.NextDouble() * 29900 + 100), 2);
        }

        public static string GenerateAccountNumber()
        {
            return new string(Enumerable.Repeat("0123456789", 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static async Task SeedDataAsync(UserManager<User> userManager, AppDbContext context)
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

            // Seed Notifications
            await SeedNotificationsAsync(context);

            // Seed Transactions
            await SeedTransactionsAsync(context);
        }

        private static async Task SeedNotificationsAsync(AppDbContext context)
        {
            if (!context.Notifications.Any())
            {
                var users = await context.Users.ToListAsync();
                if (users.Count > 0)
                {
                    foreach (var notification in SampleNotifications)
                    {
                        notification.Id = users[new Random().Next(users.Count)].Id; // Assign a random user ID
                    }
                    await context.Notifications.AddRangeAsync(SampleNotifications);
                    await context.SaveChangesAsync(); // Save notifications
                }
            }
        }

        private static async Task SeedTransactionsAsync(AppDbContext context)
        {
            if (!context.Transactions.Any())
            {
                var users = await context.Users.ToListAsync();
                if (users.Count > 0)
                {
                    var transactionsToSeed = new List<Transactions>();
                    foreach (var transaction in SampleTransactions)
                    {
                        var randomUserId = users[new Random().Next(users.Count)].Id; // Assign a random user ID
                        transactionsToSeed.Add(new Transactions
                        {
                            Id = randomUserId,
                            Amount = transaction.Amount,
                            TransactionDate = transaction.TransactionDate,
                            Reference = transaction.Reference
                        });
                    }
                    await context.Transactions.AddRangeAsync(transactionsToSeed);
                    await context.SaveChangesAsync(); // Save transactions
                }
            }
        }
    }
}