﻿using UFSBankingSystem.Data.SeedData;
using UFSBankingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UFSBankingSystem.Models.ViewModels;

namespace UFSBankingSystem.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<LoginSessions> LoginSessions { get; set; }

       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RolesConfiguration());
        }
    }
}
