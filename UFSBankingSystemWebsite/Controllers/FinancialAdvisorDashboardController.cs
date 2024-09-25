using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UFSBankingSystem.Controllers
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
            List<User> lstUsers = new List<User>();
            foreach (var user in userManager.Users)
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
                Transactions = allTransactions,
                CurrentUserBankAccount = currentUserBankAccount,
                Advise = string.Empty // Initialize Advise to avoid null values in the view
            });
        }

        // Provide advice
        [HttpPost]
        public async Task<IActionResult> ProvideAdvice(AdvisorViewModel model)
        {
            // Validate model state first
            if (!ModelState.IsValid)
            {
                Message = "Invalid data submitted.";
                return RedirectToAction("Index", "FinancialAdvisorDashboard");
            }

            var user = await userManager.FindByEmailAsync(model.UserEmail);
            if (user != null)
            {
                var allTransactions = (await wrapper.Transactions.GetAllAsync()).Where(t => t.UserEmail == model.UserEmail).ToList();
                var currentUserBankAccount = (await wrapper.BankAccount.GetAllAsync()).FirstOrDefault(ba => ba.UserEmail == model.UserEmail);

                model.CurrentUser = user;
                model.Transactions = allTransactions;
                model.CurrentUserBankAccount = currentUserBankAccount;
            }

            if (!ModelState.IsValid)
            {
                var notify = new Notification
                {
                    Message = " " + model.Advise,
                    UserEmail = model.UserEmail,
                    NotificationDate = DateTime.Now,
                    IsRead = false
                };
                await wrapper.Notification.AddAsync(notify);
                Message = "Successfully sent advice to user";
                 wrapper.SaveChanges(); // Ensure changes are saved asynchronously
                return RedirectToAction("Index", "FinancialAdvisorDashboard");
            }

            Message = "Failed to send advice to user";
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