﻿using UFSBankingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UFSBankingSystem.Data.SeedData
{
    public static class SeedData
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

        private static readonly User Customer = new User
        {
            UserName = "thabo@ufs.ac.za",
            FirstName = "Thabo",
            LastName = "Zungu",
            Email = "thabo@ufs.ac.za",
            IDnumber = 0206151810182,
            StudentStaffNumber = 7432108965,
        };

        private static readonly User Consultant = new User
        {
            UserName = "thando@ufs.ac.za",
            FirstName = "Thando",
            LastName = "Ndlela",
            Email = "thando@ufs.ac.za",
            IDnumber = 9209151587083,
            StudentStaffNumber = 9158974481,
        };

        private static readonly User FinancialAdvisor = new User
        {
            UserName = "millicent@ufs.ac.za",
            FirstName = "Millicent",
            LastName = "Kruger",
            Email = "millicent@ufs.ac.za",
            IDnumber = 9204247204082,
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

            // Seed Customer user
            await SeedUserAsync(Customer, userManager);

            // Seed Consultant user
            await SeedUserAsync(Consultant, userManager);

            // Seed Financial Advisor user
            await SeedUserAsync(FinancialAdvisor, userManager);
        }

        private static async Task SeedUserAsync(User user, UserManager<User> userManager)
        {
            // Check if the user exists; if not, create it
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                var result = await CreateDefaultAppUser(user, userManager);
                if (result.Succeeded)
                {
                    // Assign the appropriate role to the user based on their properties
                    string roleToAssign;

                    if (user.Email.Contains("jonathan"))
                        roleToAssign = "Admin";
                    else if (user.Email.Contains("thabo"))
                        roleToAssign = "User";
                    else if (user.Email.Contains("thando"))
                        roleToAssign = "Consultant";
                    else if (user.Email.Contains("millicent"))
                        roleToAssign = "FinancialAdvisor";
                    else
                        return;

                    await userManager.AddToRoleAsync(user, roleToAssign);
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