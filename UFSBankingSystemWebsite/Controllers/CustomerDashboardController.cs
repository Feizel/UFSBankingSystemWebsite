using UFSBankingSystemWebsite.Models;
using UFSBankingSystemWebsite.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UFSBankingSystemWebsite.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UFSBankingSystemWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystemWebsite.Controllers
{
    [Authorize(Roles = "User, Admin")]
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

        // Account Overview
        [HttpGet]
        public IActionResult AccountOverview()
        {
            return View();
        }

        // Account Overview
        [HttpPost]
        public async Task<IActionResult> AccountOverview(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Fetch bank accounts associated with the user
            var bankAccounts = await _context.BankAccounts
                .Where(b => b.Id == user.Id) // Assuming UserId is in BankAccount
                .ToListAsync();

            // Fetch transactions associated with the user's bank accounts
            var transactions = await _context.Transactions
                .Where(t => bankAccounts.Select(b => b.BankAccountID).Contains(t.BankAccountID))
                .ToListAsync();

            var model = new AccountOverviewViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                BankAccounts = bankAccounts,
                Transactions = transactions
            };

            return View(model);
        }

        // View Account Details
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ViewAccountDetails(int bankAccountId)
        {
            // Fetch the bank account associated with the provided ID
            var bankAccount = await _context.BankAccounts
                .Include(b => b.Transactions) // Include transactions if needed
                .FirstOrDefaultAsync(b => b.BankAccountID == bankAccountId);

            if (bankAccount == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Where(t => t.BankAccountID == bankAccount.BankAccountID)
                .ToListAsync();

            var model = new BankAccountViewModel
            {
                BankAccount = bankAccount,
                Transactions = transactions
            };

            return View(model);
        }

        // Withdraw
        [HttpGet] 
        public IActionResult Withdraw()
        {
            return View();
        }

        // Withdraw
        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(b => b.BankAccountID == model.BankAccountId && b.UserEmail == user.Email);

            if (bankAccount == null || bankAccount.Balance < model.Amount)
            {
                ModelState.AddModelError("", "Insufficient funds or account not found.");
                return View(model);
            }

            // Update balance
            bankAccount.Balance -= model.Amount;

            // Create transaction record
            var transaction = new Transactions
            {
                Id = user.Id,
                Amount = model.Amount,
                TransactionDate = DateTime.Now,
                Reference = "Withdrawal",
                BankAccountID = bankAccount.BankAccountID,
                BankAccountIdSender = bankAccount.BankAccountID,
                BankAccountIdReceiver = bankAccount.BankAccountID,
                TransactionType = TransactionType.Withdrawal,
                BalanceAfter = bankAccount.Balance
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            TempData["Message"] = "Withdrawal successful!";

            return RedirectToAction("WithdrawSuccess", new WithdrawSuccessViewModel 
            { 
                Amount = model.Amount, 
                AccountNumber = bankAccount.AccountNumber, 
                WithdrawDate = DateTime.Now
            });
        }

        //Withdraw Success
        [HttpGet]
        public IActionResult WithdrawSuccess(decimal amount, string accountNumber)
        {
            var model = new WithdrawSuccessViewModel
            {
                Amount = amount,
                AccountNumber = accountNumber,
                WithdrawDate = DateTime.Now
            };

            return View(model);
        }

        // Transfer
        [HttpGet]
        public IActionResult Transfer()
        {
            return View();
        }

        // Transfer
        [HttpPost]
        public async Task<IActionResult> Transfer(MoneyTransferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var senderAccount = await _context.BankAccounts.FindAsync(model.SenderBankAccountId);
            var receiverAccount = await _context.BankAccounts.FindAsync(model.ReceiverBankAccountId);

            if (senderAccount == null || receiverAccount == null)
            {
                ModelState.AddModelError("", "One of the accounts does not exist.");
                return View(model);
            }

            if (senderAccount.Balance < model.Amount)
            {
                ModelState.AddModelError("", "Insufficient funds in sender's account.");
                return View(model);
            }

            // Update balances
            senderAccount.Balance -= model.Amount;
            receiverAccount.Balance += model.Amount;

            // Create transaction records
            var transactionOut = new Transactions
            {
                Id = senderAccount.Id,
                Amount = model.Amount,
                TransactionDate = DateTime.Now,
                Reference = "Transfer to " + receiverAccount.AccountNumber,
                BankAccountID = senderAccount.BankAccountID,
                BankAccountIdSender = senderAccount.BankAccountID,
                BankAccountIdReceiver = receiverAccount.BankAccountID,
                TransactionType = TransactionType.Transfer,
                BalanceAfter = senderAccount.Balance
            };

            var transactionIn = new Transactions
            {
                Id = receiverAccount.Id,
                Amount = model.Amount,
                TransactionDate = DateTime.Now,
                Reference = "Transfer from " + senderAccount.AccountNumber,
                BankAccountID = receiverAccount.BankAccountID,
                BankAccountIdSender = senderAccount.BankAccountID,
                BankAccountIdReceiver = receiverAccount.BankAccountID,
                TransactionType = TransactionType.Transfer,
                BalanceAfter = receiverAccount.Balance
            };

            _context.Transactions.Add(transactionOut);
            _context.Transactions.Add(transactionIn);

            await _context.SaveChangesAsync();

            TempData["Message"] = "Transfer successful!";

            // Redirect to Transfer Success view with relevant data
            return RedirectToAction("TransferSuccess", new TransferSuccessViewModel
            {
                Amount = model.Amount,
                ReceiverAccount = receiverAccount.AccountNumber,
                SenderAccount = senderAccount.AccountNumber
            });
        }

        // Transfer Successful
        [HttpGet]
        public IActionResult TransferSuccess(decimal amount, string receiverAccount, string senderAccount)
        {
            var model = new TransferSuccessViewModel
            {
                Amount = amount,
                ReceiverAccount = receiverAccount,
                SenderAccount = senderAccount,
                TransferDate = DateTime.Now
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        // Create Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateBankAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Create a new bank account
                var newAccount = new BankAccount
                {
                    Id = user.Id, // Set foreign key to link with the user
                    AccountNumber = GenerateAccountNumber(),
                    Balance = model.InitialDeposit,
                    BankAccountType = model.AccountType,
                    UserEmail = user.Email // Ensure this is set correctly as well
                };

                // Save the new account to the database
                await _repo.BankAccount.AddAsync(newAccount);
                await _repo.SaveChangesAsync(); // Ensure changes are saved

                TempData["Message"] = "Your new bank account has been created successfully!";
                return RedirectToAction("Index", "CustomerDashboard");
            }

            return View(model); // If we got this far, something failed, redisplay form
        }

        private string GenerateAccountNumber()
        {
            // Logic to generate a unique account number
            return "ACCT" + new Random().Next(100000, 999999).ToString();
        }

        // Notifications
        public async Task<IActionResult> NotificationMessage()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction("Index", "CustomerDashboard");
            }

            var allNotifications = await _repo.Notification.GetAllAsync();
            var userNotifications = allNotifications.Where(n => n.UserEmail == user.Email).ToList();

            // Mark notifications as read
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

        // View Advice
        public async Task<IActionResult> ViewAdvice()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction("Index", "CustomerDashboard");
            }

            // Retrieve advice notifications for the current user
            var adviceList = await _repo.Notification.GetAllAsync();
            var userAdvice = adviceList.Where(n => n.UserEmail == currentUser.Email && n.Message.StartsWith("Advice:")).ToList();

            return View(userAdvice);
        }

        // Add Rating
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
            TempData["Message"] = "User not found or not logged in";
            return RedirectToAction("Index", "CustomerDashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRating(FeedBack feedback)
        {
            var currentLoginUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentLoginUser != null)
            {
                feedback.UserEmail = currentLoginUser.Email; // Ensure email is set here
                feedback.FeedbackDate = DateTime.Now;

                if (ModelState.IsValid)
                {
                    await _repo.Review.AddAsync(feedback);
                    await _repo.SaveChangesAsync(); // Ensure changes are saved

                    TempData["Message"] = "Review submitted successfully!";
                    return RedirectToAction("Index", "CustomerDashboard");
                }
            }

            TempData["Message"] = "There was an error submitting the review.";
            return View(feedback);
        }

        //public async Task<bool> Transfer(string senderAccountNumber, string receiverAccountNumber, decimal amount)
        //{

        //    var allBankAccounts = await _repo.BankAccount.GetAllAsync();

        //    var senderBankAccount = allBankAccounts.FirstOrDefault(b => b.AccountNumber == senderAccountNumber);
        //    var receiverBankAccount = allBankAccounts.FirstOrDefault(b => b.AccountNumber == receiverAccountNumber);

        //    if (senderBankAccount == null || receiverBankAccount == null)
        //    {
        //        return false;
        //    }


        //    if (senderBankAccount.Balance < amount)
        //    {
        //        return false;
        //    }


        //    senderBankAccount.Balance -= amount;
        //    receiverBankAccount.Balance += amount;


        //    await _repo.BankAccount.UpdateAsync(senderBankAccount);
        //    await _repo.BankAccount.UpdateAsync(receiverBankAccount);


        //    var transaction = new Transactions
        //    {
        //        BankAccountIdSender = int.Parse(senderBankAccount.AccountNumber), // Assuming this property exists
        //        BankAccountIdReceiver = int.Parse(receiverBankAccount.AccountNumber), // Assuming this property exists
        //        Amount = amount,
        //        TransactionDate = DateTime.UtcNow,
        //        Reference = $"Transfer from {senderBankAccount.AccountNumber} to {receiverBankAccount.AccountNumber}",
        //        UserEmail = senderBankAccount.UserEmail
        //    };
        //    await _repo.Transactions.AddAsync(transaction);

        //    var senderNotification = new Notification
        //    {
        //        Message = $"You have sent {amount:C} to account {receiverBankAccount.AccountNumber}.",
        //        NotificationDate = DateTime.UtcNow,
        //        IsRead = false,
        //        UserEmail = senderBankAccount.UserEmail
        //    };
        //    var receiverNotification = new Notification
        //    {
        //        Message = $"You have received {amount:C} from account {senderBankAccount.AccountNumber}.",
        //        NotificationDate = DateTime.UtcNow,
        //        IsRead = false,
        //        UserEmail = receiverBankAccount.UserEmail
        //    };

        //    await _repo.Notification.AddAsync(senderNotification);
        //    await _repo.Notification.AddAsync(receiverNotification);
        //    return true;
        //}

        // Transfer History
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

        // Transaction History
        //public async Task<IActionResult> TransactionHistory()
        //{
        //    var username = User.Identity.Name;
        //    var user = await _userManager.FindByNameAsync(username);

        //    if (user == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    // Get all bank accounts for the user
        //    var userBankAccounts = await _context.BankAccounts
        //        .Where(b => b.UserEmail == user.Email) // Ensure this matches how you store user identification
        //        .ToListAsync();

        //    // Get all transactions related to the user's accounts
        //    var userTransactions = await _context.Transactions
        //        .Where(t => userBankAccounts.Any(b => b.BankAccountID == t.BankAccountIdSender || b.BankAccountID == t.BankAccountIdReceiver))
        //        .OrderByDescending(t => t.TransactionDate)
        //        .ToListAsync();

        //    // Pass the transactions to the view
        //    return View(userTransactions);
        //}

        //// View Account
        //public IActionResult ViewAccount()
        //{
        //    return View();
        //}

        //// View Account
        //[HttpGet]
        //public async Task<IActionResult> ViewAccount(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    // Fetch bank account associated with the user
        //    var bankAccount = await _context.BankAccounts
        //        .FirstOrDefaultAsync(b => b.Id == user.Id); // Assuming User Id is in BankAccount

        //    // Fetch transactions associated with the bank account
        //    var transactions = await _context.Transactions
        //        .Where(t => t.BankAccountID == bankAccount.BankAccountID) // Assuming BankAccountID is in Transactions
        //        .ToListAsync();

        //    var model = new BankAccountViewModel
        //    {
        //        BankAccount = bankAccount,
        //        Transactions = transactions
        //    };

        //    return View(model);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Transfer()
        //{
        //    var username = User.Identity.Name;
        //    var user = await _userManager.FindByNameAsync(username);

        //    var allBankAccounts = await _repo.BankAccount.GetAllAsync();
        //    var mainBankAccount = allBankAccounts.FirstOrDefault(b => b.UserEmail == user.Email && b.AccountOrder == 1);

        //    var viewModel = new MoneyTransferViewModel
        //    {
        //        SenderBankAccountNumber = mainBankAccount.AccountNumber,
        //        AvailableBalance = mainBankAccount.Balance,
        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> TransferMoneyView(MoneyTransferViewModel model)
        //{
        //    var username = ViewBag.FirstName;
        //    var user = await _userManager.FindByNameAsync(username);
        //    var allBankAccounts = await _repo.BankAccount.GetAllAsync();
        //    var mainBankAccount = allBankAccounts.FirstOrDefault(b => b.UserEmail == user.Email && b.AccountOrder == 1);

        //    string senderAccountNumber = mainBankAccount.AccountNumber;
        //    string receiverAccountNumber = model.ReceiverBankAccountNumber;
        //    decimal amount = model.Amount;


        //    bool transferSuccess = await Transfer(senderAccountNumber, receiverAccountNumber, amount);

        //    if (transferSuccess)
        //    {
        //        return RedirectToAction("TransferSuccess", new { amount = amount, receiverAccount = receiverAccountNumber });
        //    }
        //    else
        //    {
        //        return View("NotFound");

        //    }
        //}

    }
}
