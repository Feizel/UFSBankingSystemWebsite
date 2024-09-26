using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UFSBankingSystem.Data;
using UFSBankingSystem.Models;
using UFSBankingSystem.Data.SeedData;
using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystemWebsite.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IFinancialAdvisorRepository, FinancialAdvisorRepository>();
builder.Services.AddScoped<IConsultantRepository, ConsultantRepository>();
builder.Services.AddScoped<IFeedBackRepository, FeedBackRepository>();

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connString)); // Using SQLite instead of SQL Server below

// Uncomment for SQL Server configuration if needed
/*
builder.Services.AddDbContext<AppDbContext>(opts =>
opts.UseSqlServer(connString, opts =>
{
    opts.EnableRetryOnFailure();
    opts.CommandTimeout(120);
    opts.UseCompatibilityLevel(110);
}));
*/

builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 8;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequireDigit = true;
    opts.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentityCore<User>().AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await SeedData.EnsurePopulatedAsync(app);

app.Run();