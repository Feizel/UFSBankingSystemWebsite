using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;

using UFSBankingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UFSBankingSystem.Controllers
{
    public class CustomerDashboardController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly UserManager<User> _userManager;

        public CustomerDashboardController(IRepositoryWrapper repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }
        [TempData]
        public string Message { get; set; }
        public async Task<IActionResult> IndexAsync()
        {
            // Get the logged-in user's username
            var username = User.Identity.Name;


            var user = await _userManager.FindByNameAsync(username);

            // Get all bank accounts for the user
            var allBankAccounts = await _repo.BankAccount.GetAllAsync();


            var userBankAccounts = allBankAccounts.Where(b => b.UserEmail == user.Email).ToList();


            var transactions = await _repo.Transactions.GetAllAsync();
            var userTransactions = transactions.Where(t => userBankAccounts.Any(b => b.UserEmail == user.Email)).ToList();


            var viewModel = new BankAccountViewModel
            {
                BankAccount = userBankAccounts,
                Transactions = userTransactions
            };

            // Return the view with the viewModel
            return View(viewModel);
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
                return RedirectToAction("Index", "Client");
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
            return RedirectToAction("Index", "Client");
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
                    return RedirectToAction("Index", "Client");
                }
            }

            Message = "There was an error sending the review";
            return View(feedback);
        }





        public async Task<bool> TransferMoney(string senderAccountNumber, string receiverAccountNumber, decimal amount)
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
        public async Task<IActionResult> TransferMoneyView()
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


            bool transferSuccess = await TransferMoney(senderAccountNumber, receiverAccountNumber, amount);

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
