using UFSBankingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UFSBankingSystem.Data.SeedData
{
    public static class SeedData
    {
        private static readonly string password = "Teboho123!";
        private static readonly AppUser consultantUser = new AppUser
        {
            UserName = "def_consultant",
            FirstName = "Prudance",
            LastName = "Smith",
            Email = "smith@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "9209151587426",
            StudentStaffNumber = "0123456789",
            AccountNumber = "0000000001",
            UserRole = "Consultant"
        };
        private static readonly AppUser adminUser = new AppUser
        {
            UserName = "def_admin",
            FirstName = "William",
            LastName = "Henry",
            Email = "henry@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "8876543210123",
            StudentStaffNumber = "9876543210",
            AccountNumber = "0000000002",
            UserRole = "Admin"
        };
        private static readonly AppUser finAdviserUser = new AppUser
        {
            UserName = "def_advisor",
            FirstName= "Innocent",
            LastName= "Miller",
            Email = "miller@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "9811227204013",
            StudentStaffNumber = "9876543210",
            AccountNumber = "0000000003",
            UserRole = "FAdvisor"
        };
        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices.CreateScope()
               .ServiceProvider.GetRequiredService<AppDbContext>();

            UserManager<AppUser> userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            if (await userManager.FindByNameAsync(consultantUser.UserName) == null)
            {

                if (await roleManager.FindByNameAsync(consultantUser.UserRole) == null)
                    await roleManager.CreateAsync(new(consultantUser.UserRole));

                IdentityResult result = await CreatePreAppUser(consultantUser, userManager);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(consultantUser, "Consultant");
                result = await CreatePreAppUser(adminUser, userManager);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            if(await userManager.FindByEmailAsync(finAdviserUser.UserName) == null)
            {
                if( await roleManager.FindByNameAsync(finAdviserUser.UserRole)== null)
                    await roleManager.CreateAsync(new(finAdviserUser.UserRole));
                IdentityResult result = await CreatePreAppUser(finAdviserUser, userManager);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(finAdviserUser, "FAdvisor");
                result = await CreatePreAppUser(adminUser, userManager);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        public static async Task<IdentityResult> CreatePreAppUser(AppUser user, UserManager<AppUser> userManager)
        {
            AppUser _user = new()
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