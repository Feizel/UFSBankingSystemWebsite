using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UFSBankingSystemWebsite.Data;
using UFSBankingSystemWebsite.Models;
using UFSBankingSystemWebsite.Data.SeedData;
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

// Configure SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPath = Path.GetFullPath(connectionString.Replace("Data Source=", ""));
var dbDirectory = Path.GetDirectoryName(dbPath);

if (!Directory.Exists(dbDirectory))
{
    Directory.CreateDirectory(dbDirectory);
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

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

// Database initialization and seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        // Check if the database exists and delete it
        if (File.Exists(dbPath))
        {
            logger.LogInformation("Deleting existing database...");
            context.Database.EnsureDeleted();
        }

        // Create a new database and apply migrations
        logger.LogInformation("Creating new database and applying migrations...");
        context.Database.Migrate();

        // Seed data
        logger.LogInformation("Seeding data...");
        await SeedData.EnsurePopulatedAsync(app);

        logger.LogInformation("Database setup completed successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while setting up or seeding the database.");
    }
}

app.Run();