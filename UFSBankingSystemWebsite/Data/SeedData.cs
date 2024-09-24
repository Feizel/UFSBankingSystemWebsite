using UFSBankingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UFSBankingSystem.Data.SeedData
{
    public static class SeedData
    {
        private static readonly string password = "@TestApp123";
        private static readonly User Admin = new User
        {
            UserName = "_Admin",
            FirstName = "Jonathan",
            LastName = "Meyers",
            Email = "jonathan@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "8876543210123",
            StudentStaffNumber = "9876543210",
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
            IDnumber = "0206151810182",
            StudentStaffNumber = "7432108965",
            AccountNumber = "0000000002",
            UserRole = "Admin"
        };
        private static readonly User Consultant = new User
        {
            UserName = "_Consultant",
            FirstName = "Thando",
            LastName = "Ndlela",
            Email = "thando@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "9209151587083",
            StudentStaffNumber = "9158974481",
            AccountNumber = "0000000003",
            UserRole = "Consultant"
        };
        private static readonly User FinancialAdvisor = new User
        {
            UserName = "_Financial Advisor",
            FirstName= "Millicent",
            LastName= "Kruger",
            Email = "millicent@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "9204247204082",
            StudentStaffNumber = "9876543210",
            AccountNumber = "0000000004",
            UserRole = "FinancialAdvisor"
        };
        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices.CreateScope()
               .ServiceProvider.GetRequiredService<AppDbContext>();

            UserManager<User> userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            if (await userManager.FindByNameAsync(Consultant.UserName) == null)
            {

                if (await roleManager.FindByNameAsync(Consultant.UserRole) == null)
                    await roleManager.CreateAsync(new(Consultant.UserRole));

                IdentityResult result = await CreateDefaultAppUser(Consultant, userManager);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(Consultant, "Consultant");
                result = await CreateDefaultAppUser(Admin, userManager);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(Admin, "Admin");
            }
            if(await userManager.FindByEmailAsync(FinancialAdvisor.UserName) == null)
            {
                if( await roleManager.FindByNameAsync(FinancialAdvisor.UserRole)== null)
                    await roleManager.CreateAsync(new(FinancialAdvisor.UserRole));
                IdentityResult result = await CreateDefaultAppUser(FinancialAdvisor, userManager);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(FinancialAdvisor, "FAdvisor");
                result = await CreateDefaultAppUser(Admin, userManager);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(Admin, "Admin");
            }
        }
        public static async Task<IdentityResult> CreateDefaultAppUser(User user, UserManager<User> userManager)
        {
            User _user = new()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                StudentStaffNumber = user.StudentStaffNumber,
                AccountNumber = user.AccountNumber,
                DateOfBirth = user.DateOfBirth,
                IDnumber = user.IDnumber,
                UserRole = user.UserRole,
            };
            return await userManager.CreateAsync(user, password);
        }
    }
}