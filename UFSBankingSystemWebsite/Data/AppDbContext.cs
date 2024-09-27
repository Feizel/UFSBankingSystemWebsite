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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<LoginSession> LoginSessions { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<FinancialAdvisor> FinancialAdvisors { get; set; }
        public DbSet<FinancialAdvice> FinancialAdvices { get; set; }
        public DbSet<Investment> Investments { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RolesConfiguration());

            // Configure table names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<BankAccount>().ToTable("Accounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<FeedBack>().ToTable("Feedbacks");
            modelBuilder.Entity<Consultant>().ToTable("Consultants");
            modelBuilder.Entity<FinancialAdvisor>().ToTable("FinancialAdvisors");
            modelBuilder.Entity<LoginSession>().ToTable("LoginSessions");

            // Configure decimal precision
            modelBuilder.Entity<BankAccount>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            // Configure relationships
            modelBuilder.Entity<BankAccount>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.BankAccountID);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<FeedBack>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(f => f.UserEmail); // Assuming UserEmail is used as a foreign key

            modelBuilder.Entity<LoginSession>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId); // Assuming UserEmail is used as a foreign key

            // Additional configurations for other entities...
        }
    }
}