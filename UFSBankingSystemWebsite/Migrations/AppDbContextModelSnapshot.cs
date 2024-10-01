﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UFSBankingSystemWebsite.Data;

#nullable disable

namespace UFSBankingSystemWebsite.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "73ceec7c-bdb9-46ac-bf7a-b5c801e2a54d",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "405f2d96-54f2-493d-bcfd-329996293c7a",
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "873b27db-29a2-4f5e-b998-bf1a9219aeb4",
                            Name = "Consultant",
                            NormalizedName = "CONSULTANT"
                        },
                        new
                        {
                            Id = "d630f928-edc3-47f4-9dfb-2ec6a2f0625d",
                            Name = "FinancialAdvisor",
                            NormalizedName = "FINANCIALADVISOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.BankAccount", b =>
                {
                    b.Property<int>("BankAccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("AccountOrder")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("BankAccountType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserEmail")
                        .HasColumnType("TEXT");

                    b.HasKey("BankAccountID");

                    b.HasIndex("Id");

                    b.ToTable("BankAccounts", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Consultant", b =>
                {
                    b.Property<int>("ConsultantID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmployeeNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ConsultantID");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Consultants", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.FeedBack", b =>
                {
                    b.Property<int>("FeedBackID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("FeedbackDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("FeedBackID");

                    b.HasIndex("UserId");

                    b.ToTable("Feedbacks", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.FinancialAdvice", b =>
                {
                    b.Property<int>("FinancialAdviceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Advice")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("FinancialAdvisorID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("FinancialAdviceID");

                    b.HasIndex("FinancialAdvisorID");

                    b.HasIndex("UserId");

                    b.ToTable("FinancialAdvices", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.FinancialAdvisor", b =>
                {
                    b.Property<int>("FinancialAdvisorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmployeeNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("FinancialAdvisorID");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("FinancialAdvisors", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.LoginSession", b =>
                {
                    b.Property<int>("LoginSessionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("LoginSessionID");

                    b.HasIndex("UserId");

                    b.ToTable("LoginSessions", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsRead")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("NotificationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserEmail")
                        .HasColumnType("TEXT");

                    b.HasKey("NotificationID");

                    b.HasIndex("Id");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Report", b =>
                {
                    b.Property<int>("ReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConsultantID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("ReportID");

                    b.HasIndex("ConsultantID");

                    b.ToTable("Reports", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Transactions", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("BalanceAfter")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BankAccountID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BankAccountIdReceiver")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BankAccountIdSender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("TransactionType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserEmail")
                        .HasColumnType("TEXT");

                    b.HasKey("TransactionID");

                    b.HasIndex("BankAccountID");

                    b.HasIndex("Id");

                    b.ToTable("Transactions", (string)null);
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IDnumber")
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsConsultant")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCustomer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentStaffNumber")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserRole")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UFSBankingSystemWebsite.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.BankAccount", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", "User")
                        .WithMany("BankAccounts")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Consultant", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", "User")
                        .WithOne("Consultants")
                        .HasForeignKey("UFSBankingSystemWebsite.Models.Consultant", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.FeedBack", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", "User")
                        .WithMany("FeedBacks")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.FinancialAdvice", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.FinancialAdvisor", "FinancialAdvisor")
                        .WithMany("FinancialAdvices")
                        .HasForeignKey("FinancialAdvisorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UFSBankingSystemWebsite.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("FinancialAdvisor");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.FinancialAdvisor", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", "User")
                        .WithOne("FinancialAdvisors")
                        .HasForeignKey("UFSBankingSystemWebsite.Models.FinancialAdvisor", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.LoginSession", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Notification", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Report", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.Consultant", "Consultant")
                        .WithMany("Reports")
                        .HasForeignKey("ConsultantID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consultant");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Transactions", b =>
                {
                    b.HasOne("UFSBankingSystemWebsite.Models.BankAccount", "BankAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("BankAccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UFSBankingSystemWebsite.Models.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("BankAccount");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.BankAccount", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.Consultant", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.FinancialAdvisor", b =>
                {
                    b.Navigation("FinancialAdvices");
                });

            modelBuilder.Entity("UFSBankingSystemWebsite.Models.User", b =>
                {
                    b.Navigation("BankAccounts");

                    b.Navigation("Consultants");

                    b.Navigation("FeedBacks");

                    b.Navigation("FinancialAdvisors");

                    b.Navigation("Notifications");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
