using UFSBankingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFSBankingSystem.Data.SeedData
{
    public static class SeedDataD
    {
        private static readonly string password = "@TestApp123";

        private static readonly User Admin = new User
        {
            UserName = "jonathan@ufs.ac.za",
            FirstName = "Jonathan",
            LastName = "Meyers",
            Email = "jonathan@ufs.ac.za",
            IDnumber = 8876543210123,
            StudentStaffNumber = 9876543210,
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
            // Seed roles
            string[] roles = { "Admin", "Consultant", "FinancialAdvisor", "User" };

            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin user
            await SeedUserAsync(Admin, userManager);

            // Seed sample customers and staff
            foreach (var customer in SampleData.SampleCustomers)
            {
                await SeedUserAsync(customer, userManager);
            }

            foreach (var staff in SampleData.SampleStaff)
            {
                await SeedUserAsync(staff, userManager);
            }
        }

        private static async Task SeedUserAsync(User user, UserManager<User> userManager)
        {
            // Check if the user exists; if not, create it
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                var result = await CreateDefaultAppUser(user, userManager);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                else
                {
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