using UFSBankingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UFSBankingSystem.Data.SeedData
{
    public static class SeedData2
    {
        private static readonly string password = "@TestApp123";

        private static readonly User Admin = new User
        {
            UserName = "Master Admin",
            FirstName = "Jonathan",
            LastName = "Meyers",
            Email = "jonathan@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = 8876543210123,
            StudentStaffNumber = 9876543210,
            AccountNumber = "0000000001",
            UserRole = "Admin"
        };

        private static readonly User Customer = new User
        {
            UserName = "_Customer",
            FirstName = "Thabo",
            LastName = "Zungu",
            Email = "thabo@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = 0206151810182,
            StudentStaffNumber = 7432108965,
            AccountNumber = "0000000002",
            UserRole = "User"
        };

        private static readonly User Consultant = new User
        {
            UserName = "Master Consultant",
            FirstName = "Thando",
            LastName = "Ndlela",
            Email = "thando@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = 9209151587083,
            StudentStaffNumber = 9158974481,
            AccountNumber = "0000000003",
            UserRole = "Consultant"
        };

        private static readonly User FinancialAdvisor = new User
        {
            UserName = "Master FinancialAdvisor",
            FirstName = "Millicent",
            LastName = "Kruger",
            Email = "millicent@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = 9204247204082,
            StudentStaffNumber = 9876543210,
            AccountNumber = "0000000004",
            UserRole = "FinancialAdvisor"
        };

        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Apply any pending migrations
            if (context.Database.GetPendingMigrations().Any())
                await context.Database.MigrateAsync();

            // Seed roles and users
            await SeedRolesAndUsersAsync(userManager, roleManager);
        }

        private static async Task SeedRolesAndUsersAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Admin role and user
            await SeedUserAsync(Admin, userManager, roleManager);

            // Seed Customer role and user
            await SeedUserAsync(Customer, userManager, roleManager);

            // Seed Consultant role and user
            await SeedUserAsync(Consultant, userManager, roleManager);

            // Seed Financial Advisor role and user
            await SeedUserAsync(FinancialAdvisor, userManager, roleManager);
        }

        private static async Task SeedUserAsync(User user, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if the role exists; if not, create it
            if (await roleManager.FindByNameAsync(user.UserRole) == null)
                await roleManager.CreateAsync(new IdentityRole(user.UserRole));

            // Check if the user exists; if not, create it
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                var result = await CreateDefaultAppUser(user, userManager);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, user.UserRole);
                }
                else
                {
                    // Handle errors (e.g., log them)
                    throw new Exception($"Failed to create user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        public static async Task<IdentityResult> CreateDefaultAppUser(User user, UserManager<User> userManager)
        {
            return await userManager.CreateAsync(user, password);
        }
    }
}