using UFSBankingSystemWebsite.Models;
using UFSBankingSystemWebsite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystemWebsite.Controllers
{
    [Authorize(Roles = "FinancialAdvisor, Admin")]
    public class FinancialAdvisorDashboardController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IRepositoryWrapper wrapper;

        public FinancialAdvisorDashboardController(UserManager<User> _userManager, IRepositoryWrapper wrapper)
        {
            userManager = _userManager;
            this.wrapper = wrapper;
        }


        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> Index()
        {
            // Get the logged-in user's username
            var username = User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            // Set ViewBag.FirstName for use in the layout
            ViewBag.FirstName = user.FirstName;

            List<User> lstUsers = new List<User>();
            foreach (var lstUser in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "User"))
                    lstUsers.Add(user);
            }
            return View(new ConsultantViewModel
            {
                appUsers = lstUsers.AsQueryable()
            });

        }

        // Provide advice
        [HttpGet]
        public async Task<IActionResult> ProvideAdvice(string email)
        {
            // Check if email is null or empty
            if (string.IsNullOrEmpty(email))
            {
                Message = "Email parameter is missing.";
                return RedirectToAction("Index", "FinancialAdvisorDashboard");
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Message = "Could not Find User, Please Try Again";
                return RedirectToAction("Index", "FinancialAdvisorDashboard");
            }

            var allTransactions = (await wrapper.Transactions.GetAllAsync()).Where(t => t.UserEmail == email).ToList();
            var currentUserBankAccount = (await wrapper.BankAccount.GetAllAsync()).FirstOrDefault(ba => ba.UserEmail == email);

            return View(new AdvisorViewModel
            {
                UserEmail = user.Email,
                CurrentUser = user,
                Transactions = allTransactions ?? new List<Transactions>(), // Initialize to an empty list if null
                CurrentUserBankAccount = currentUserBankAccount ?? new BankAccount(), // Initialize to a new Account object if null
                Advise = string.Empty // Initialize Advise to avoid null values in the view
            });

            return View(new AdvisorViewModel
            {
                UserEmail = user.Email,
                CurrentUser = user,
                Transactions = allTransactions,
                CurrentUserBankAccount = currentUserBankAccount,
                Advise = string.Empty // Initialize Advise to avoid null values in the view
            });
        }

        [HttpPost]
        public async Task<IActionResult> ProvideAdvice(AdvisorViewModel model)
        {
            // Validate model state first
            if (!ModelState.IsValid)
            {
                Message = "Invalid data submitted.";
                return RedirectToAction("Index", "FinancialAdvisorDashboard");
            }

            // Find the user by email
            var user = await userManager.FindByEmailAsync(model.UserEmail);
            if (user == null)
            {
                Message = "User not found.";
                return RedirectToAction("Index", "FinancialAdvisorDashboard");
            }

            // Create a notification for the advice
            var notify = new Notification
            {
                Message = model.Advise, // Use the advise from the model
                UserEmail = model.UserEmail,
                NotificationDate = DateTime.Now,
                IsRead = false
            };

            // Add notification to the repository
            await wrapper.Notification.AddAsync(notify);

            // Save changes asynchronously
            wrapper.SaveChanges();

            // Set success message
            Message = "Successfully sent advice to user";

            return RedirectToAction("Index", "FinancialAdvisorDashboard");
        }

        // View Customer Accounts
        public async Task<IActionResult> ViewCustomerAccounts()
        {
            var users = await wrapper.AppUser.FindAllAsync(); // Fetch all users
            return View(users);
        }


    }
}