using UFSBankingSystemWebsite.Data.SeedData;
using UFSBankingSystemWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UFSBankingSystemWebsite.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<FinancialAdvisor> FinancialAdvisors { get; set; }
        public DbSet<FinancialAdvice> FinancialAdvices { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<LoginSession> LoginSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RolesConfiguration());

            // Configure relationships
            modelBuilder.Entity<User>()
           .HasMany(u => u.BankAccounts)
           .WithOne(a => a.User)
           .HasForeignKey(a => a.Id)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Consultants)
                .WithOne(c => c.User)
                .HasForeignKey<Consultant>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.FinancialAdvisors)
                .WithOne(fa => fa.User)
                .HasForeignKey<FinancialAdvisor>(fa => fa.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // Configure table names (if needed)
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<BankAccount>().ToTable("BankAccounts");
            modelBuilder.Entity<Transactions>().ToTable("Transactions");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<Consultant>().ToTable("Consultants");
            modelBuilder.Entity<FinancialAdvisor>().ToTable("FinancialAdvisors");
            modelBuilder.Entity<FinancialAdvice>().ToTable("FinancialAdvices");
            modelBuilder.Entity<Report>().ToTable("Reports");
            modelBuilder.Entity<FeedBack>().ToTable("Feedbacks");
            modelBuilder.Entity<LoginSession>().ToTable("LoginSessions");
        }
    }
}