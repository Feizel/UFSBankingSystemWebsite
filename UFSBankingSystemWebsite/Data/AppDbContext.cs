using UFSBankingSystem.Data.SeedData;
using UFSBankingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UFSBankingSystem.Models.ViewModels;

namespace UFSBankingSystem.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> BankAccounts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<LoginSessions> LoginSessions { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.ApplyConfiguration(new RolesConfiguration());
        }
    }
}
