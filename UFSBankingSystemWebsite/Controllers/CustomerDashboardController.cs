using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UFSBankingSystem.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UFSBankingSystem.Data;
using Microsoft.AspNetCore.Authorization;

namespace UFSBankingSystem.Controllers
{
    [Authorize(Roles = "User")]
    public class CustomerDashboardController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public CustomerDashboardController(IRepositoryWrapper repo, UserManager<User> userManager, AppDbContext context)
        {
            _repo = repo;
            _userManager = userManager;
            _context = context;
        }
        [TempData]
        public string Message { get; set; }
        public async Task<IActionResult> Index()
        {
            // Get the logged-in user's username
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            // Set ViewBag.FirstName for use in the layout
            ViewBag.FirstName = user.FirstName;

            // Get all bank accounts for the user
            var allBankAccounts = await _repo.BankAccount.GetAllAsync();
            var userBankAccounts = allBankAccounts.Where(b => b.UserEmail == user.Email).ToList();


            var transactions = await _repo.Transactions.GetAllAsync();
            var userTransactions = transactions.Where(t => userBankAccounts.Any(b => b.UserEmail == user.Email)).ToList();

            // View model to pass to the view
            var viewModel = new CustomerViewModel
            {
                BankAccounts = userBankAccounts,
                Transactions = userTransactions
            };

            // Return the view with the viewModel
            return View(viewModel);
        }
        //public IActionResult ViewAccount()
        //{
        //    return View();
        //}
        public async Task<IActionResult> ViewAccount(string id)
        {
            var account = await _context.BankAccounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions.Where(t => t.AccountID == int.Parse(id)).ToListAsync();

            var model = new BankAccountViewModel
            {
                BankAccount = account,
                Transactions = transactions
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        //Create Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new bank account
                var newAccount = new Account
                {
                    AccountNumber = GenerateAccountNumber(), // Method to generate a unique account number
                    Balance = model.InitialDeposit,
                    BankAccountType = model.AccountType,
                    AccountName = model.AccountName, // Set the account name from the view model
                    UserEmail = User.Identity.Name // Assuming the user is logged in and their email is their username
                };

                // Save the new account to the database
                await _repo.BankAccount.CreateAsync(newAccount);
                _repo.SaveChanges(); // Ensure changes are saved

                TempData["Message"] = "Your account has been created successfully!";
                return RedirectToAction("Index", "CustomerDashboard");
            }

            return View(model);
        }

        private string GenerateAccountNumber()
        {
            // Logic to generate a unique account number
            return "ACCT" + new Random().Next(100000, 999999).ToString();
        }

        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User) as User;

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.StudentStaffNumber = model.StudentNumber;
                user.StudentStaffNumber = model.EmployeeNumber;
                user.IDnumber = model.IDNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "CustomerDashboard");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }
        public async Task<IActionResult> NotificationMessage()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            var allNotifications = await _repo.Notification.GetAllAsync();
            var userNotifications = allNotifications.Where(n => n.UserEmail == user.Email).ToList();

            foreach (var notification in userNotifications)
            {
                if (!notification.IsRead)
                {
                    notification.IsRead = true;
                    await _repo.Notification.UpdateAsync(notification);
                }
            }

            return View(userNotifications);
        }


        public async Task<IActionResult> ViewAdvice()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                Message = "User not found.";
                return RedirectToAction("Index", "CustomerDashboard");
            }

            // Retrieve advice notifications for the current user
            var adviceList = await _repo.Notification.GetAllAsync();
            var userAdvice = adviceList.Where(n => n.UserEmail == currentUser.Email && n.Message.StartsWith(" ")).ToList();

            return View(userAdvice);
        }



        [HttpGet]
        public async Task<IActionResult> AddRating()
        {
            var currentLoginUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentLoginUser != null)
            {
                var feedback = new FeedBack
                {
                    UserEmail = currentLoginUser.Email // Pre-populate email field
                };
                return View(feedback);
            }

            // Handle case where the user is not logged in or not found
            Message = "User not found or not logged in";
            return RedirectToAction("Index", "CustomerDashboard");
        }
        [HttpPost]
        public async Task<IActionResult> AddRating(FeedBack feedback)
        {
            var currentLoginUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentLoginUser != null)
            {
                feedback.UserEmail = currentLoginUser.Email; // Ensure email is set here
                feedback.dateTime = DateTime.Now;

                if (ModelState.IsValid)
                {
                    await _repo.Review.AddAsync(feedback);
                    Message = "Review sent successfully";
                    return RedirectToAction("Index", "CustomerDashboard");
                }
            }

            Message = "There was an error sending the review";
            return View(feedback);
        }
        public async Task<bool> Transfer(string senderAccountNumber, string receiverAccountNumber, decimal amount)
        {

            var allBankAccounts = await _repo.BankAccount.GetAllAsync();

            var senderBankAccount = allBankAccounts.FirstOrDefault(b => b.AccountNumber == senderAccountNumber);
            var receiverBankAccount = allBankAccounts.FirstOrDefault(b => b.AccountNumber == receiverAccountNumber);

            if (senderBankAccount == null || receiverBankAccount == null)
            {
                return false;
            }


            if (senderBankAccount.Balance < amount)
            {
                return false;
            }


            senderBankAccount.Balance -= amount;
            receiverBankAccount.Balance += amount;


            await _repo.BankAccount.UpdateAsync(senderBankAccount);
            await _repo.BankAccount.UpdateAsync(receiverBankAccount);


            var transaction = new Transaction
            {
                BankAccountIdSender = int.Parse(senderBankAccount.AccountNumber), // Assuming this property exists
                BankAccountIdReceiver = int.Parse(receiverBankAccount.AccountNumber), // Assuming this property exists
                Amount = amount,
                TransactionDate = DateTime.UtcNow,
                Reference = $"Transfer from {senderBankAccount.AccountNumber} to {receiverBankAccount.AccountNumber}",
                UserEmail = senderBankAccount.UserEmail
            };
            await _repo.Transactions.AddAsync(transaction);

            var senderNotification = new Notification
            {
                Message = $"You have sent {amount:C} to account {receiverBankAccount.AccountNumber}.",
                NotificationDate = DateTime.UtcNow,
                IsRead = false,
                UserEmail = senderBankAccount.UserEmail
            };
            var receiverNotification = new Notification
            {
                Message = $"You have received {amount:C} from account {senderBankAccount.AccountNumber}.",
                NotificationDate = DateTime.UtcNow,
                IsRead = false,
                UserEmail = receiverBankAccount.UserEmail
            };

            await _repo.Notification.AddAsync(senderNotification);
            await _repo.Notification.AddAsync(receiverNotification);
            return true;
        }

        [HttpGet]
        public async Task<IActionResult> Transfer()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            var allBankAccounts = await _repo.BankAccount.GetAllAsync();
            var mainBankAccount = allBankAccounts.FirstOrDefault(b => b.UserEmail == user.Email && b.AccountOrder == 1);

            var viewModel = new MoneyTransferViewModel
            {
                SenderBankAccountNumber = mainBankAccount.AccountNumber,
                AvailableBalance = mainBankAccount.Balance,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TransferMoneyView(MoneyTransferViewModel model)
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            var allBankAccounts = await _repo.BankAccount.GetAllAsync();
            var mainBankAccount = allBankAccounts.FirstOrDefault(b => b.UserEmail == user.Email && b.AccountOrder == 1);

            string senderAccountNumber = mainBankAccount.AccountNumber;
            string receiverAccountNumber = model.ReceiverBankAccountNumber;
            decimal amount = model.Amount;


            bool transferSuccess = await Transfer(senderAccountNumber, receiverAccountNumber, amount);

            if (transferSuccess)
            {
                return RedirectToAction("TransferSuccess", new { amount = amount, receiverAccount = receiverAccountNumber });
            }
            else
            {
                return View("NotFound");

            }
        }
        public IActionResult TransferSuccess(decimal amount, string receiverAccount)
        {
            var model = new TransferSuccessViewModel
            {
                Amount = amount,
                ReceiverAccount = receiverAccount
            };
            return View(model);
        }

        public async Task<IActionResult> TransactionHistory()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get all bank accounts for the user
            var allBankAccounts = await _repo.BankAccount.GetAllAsync();
            var userBankAccounts = allBankAccounts.Where(b => b.UserEmail == user.Email).ToList();

            // Get all transactions related to the user's accounts
            var allTransactions = await _repo.Transactions.GetAllAsync();
            var userTransactions = allTransactions
                .Where(t => userBankAccounts.Any(b => b.AccountNumber == t.BankAccountIdSender.ToString() || b.AccountNumber == t.BankAccountIdReceiver.ToString()))
                .OrderByDescending(t => t.TransactionDate)
                .ToList();

            // Pass the transactions to the view
            return View(userTransactions);
        }

        // GET: Display the rating form
        [HttpGet]
        public IActionResult UserRatings()
        {
            return View("UserRatings");
        }

        // POST: Handle the rating submission
        
    }
}
