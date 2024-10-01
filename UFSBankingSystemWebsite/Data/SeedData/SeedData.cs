using UFSBankingSystemWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data;
using UFSBankingSystemWebsite.Data.SeedData;

namespace UFSBankingSystemWebsite.Data.SeedData
{
    public static class SeedData
    {
        private static readonly string password = "@TestApp123";
        private static readonly Random random = new Random();

        private static readonly User Admin = new User
        {
            UserName = "admin@ufs.ac.za",
            FirstName = "Jonathan",
            LastName = "Meyers",
            Email = "admin@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "8876543210123",
            StudentStaffNumber = "9876543210",
            AccountNumber = "0000000001",
            UserRole = "Admin"
        };

        private static readonly User Customer = new User
        {
            UserName = "testCustomero@ufs.ac.za",
            FirstName = "Thabo",
            LastName = "Zungu",
            Email = "testCustomero@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "0206151810182",
            StudentStaffNumber = "7432108965",
            AccountNumber = "0000000002",
            UserRole = "User"
        };

        private static readonly User Consultant = new User
        {
            UserName = "consultant@ufs.ac.za",
            FirstName = "Thando",
            LastName = "Ndlela",
            Email = "consultant@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "9209151587083",
            StudentStaffNumber = "9158974481",
            AccountNumber = "0000000003",
            UserRole = "Consultant"
        };

        private static readonly User FinancialAdvisor = new User
        {
            UserName = "finAdvisor@ufs.ac.za",
            FirstName = "Millicent",
            LastName = "Kruger",
            Email = "finAdvisor@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "9204247204082",
            StudentStaffNumber = "9876543210",
            AccountNumber = "0000000004",
            UserRole = "FinancialAdvisor"
        };

        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (context.Database.GetPendingMigrations().Any())
                    await context.Database.MigrateAsync();

                // Seed roles
                string[] roles = new[] { "Admin", "User", "Consultant", "FinancialAdvisor" };
                foreach (string role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Seed users
                await SeedUser(userManager, context, Admin);
                await SeedUser(userManager, context, Customer);
                await SeedUser(userManager, context, Consultant);
                await SeedUser(userManager, context, FinancialAdvisor);

                // Seed sample data
                await SeedSampleData(context, userManager);

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUser(UserManager<User> userManager, AppDbContext context, User user)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                var result = await userManager.CreateAsync(user, SampleData.DefaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, user.UserRole);
                    await CreateBankAccounts(context, user);
                }
            }

            //if (await userManager.FindByEmailAsync(user.Email) == null)
            //{
            //    var result = await userManager.CreateAsync(user, password);
            //    if (result.Succeeded)
            //        await userManager.AddToRoleAsync(user, user.UserRole);
            //}
        }

        private static async Task CreateBankAccounts(AppDbContext context, User user)
        {
            string[] accountTypes = { "Fixed", "Savings", "Cheque" };
            int order = 1;

            foreach (var accountType in accountTypes)
            {
                var bankAccount = new BankAccount
                {
                    Id = user.Id,
                    AccountNumber = SampleData.GenerateAccountNumber(),
                    Balance = SampleData.GetRandomBalance(),
                    BankAccountType = accountType,
                    AccountOrder = order++,
                    UserEmail = user.Email,
                    User = user
                };

                context.BankAccounts.Add(bankAccount);
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedSampleData(AppDbContext context, UserManager<User> userManager)
        {
            if (!context.Users.Any(u => SampleData.SampleCustomers.Select(sc => sc.Email).Contains(u.Email)))
            {
                //await context.Users.AddRangeAsync(SampleData.SampleCustomers);
                //await context.Users.AddRangeAsync(SampleData.SampleStaff);

                foreach (var user in SampleData.SampleCustomers.Concat(SampleData.SampleStaff))
                {
                    await SeedUser(userManager, context, user);
                }

                await context.SaveChangesAsync(); // Save users first
            }

            if (!context.Transactions.Any())
            {
                await context.Transactions.AddRangeAsync(SampleData.SampleTransactions);
                await context.SaveChangesAsync(); // Save transactions
            }

            if (!context.Notifications.Any())
            {
                var users = await context.Users.ToListAsync(); // Fetch all users
                if (users.Count > 0) // Ensure there are users available
                {
                    foreach (var notification in SampleData.SampleNotifications)
                    {
                        // Assign a random user ID from the list of users to each notification
                        notification.Id = users[new Random().Next(users.Count)].Id;
                    }
                    await context.Notifications.AddRangeAsync(SampleData.SampleNotifications);
                    await context.SaveChangesAsync(); // Save notifications
                }
            }

            //if (!context.Notifications.Any())
            //{
            //    var users = await context.Users.ToListAsync();
            //    foreach (var notification in SampleData.SampleNotifications)
            //    {
            //        notification.Id = users[new Random().Next(users.Count)].Id;
            //    }
            //    await context.Notifications.AddRangeAsync(SampleData.SampleNotifications);
            //    await context.SaveChangesAsync(); // Save notifications
            //}
        }
    }
}