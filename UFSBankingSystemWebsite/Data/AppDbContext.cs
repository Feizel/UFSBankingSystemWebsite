using UFSBankingSystem.Data.SeedData;
using UFSBankingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UFSBankingSystemWebsite.Data.SeedData;

namespace UFSBankingSystem.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
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

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.BankAccounts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Consultant)
                .WithOne(c => c.User)
                .HasForeignKey<Consultant>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.FinancialAdvisor)
                .WithOne(fa => fa.User)
                .HasForeignKey<FinancialAdvisor>(fa => fa.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BankAccount>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.BankAccount)
                .HasForeignKey(t => t.BankAccountID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FinancialAdvisor>()
                .HasMany(fa => fa.FinancialAdvices)
                .WithOne(a => a.FinancialAdvisor)
                .HasForeignKey(a => a.FinancialAdvisorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Consultant>()
                .HasMany(c => c.Reports)
                .WithOne(r => r.Consultant)
                .HasForeignKey(r => r.ConsultantID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FeedBack>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoginSession>()
                .HasOne(ls => ls.User)
                .WithMany()
                .HasForeignKey(ls => ls.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure table names (if needed)
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<BankAccount>().ToTable("BankAccounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
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