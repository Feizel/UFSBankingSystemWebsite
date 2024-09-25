using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using UFS_Banking_System_Website.Models.ViewModels;

namespace UFSBankingSystem.Controllers
{
   
    [Authorize(Roles = "Consultant,Admin")]
    public class ConsultantDashboardController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repository;

        public ConsultantDashboardController(UserManager<User> _userManager, IRepositoryWrapper _repository)
        {
            _userManager = _userManager;
            this._repository = _repository;
        }


        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> Index()
        {
            List<User> lstUsers = new List<User>();
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "User"))
                    lstUsers.Add(user);
            }
            return View(new ConsultantViewModel
            {
                appUsers = lstUsers.AsQueryable()
            });
        }

        public async Task<IActionResult> ViewAllLogins(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var allLogins = await _repository.Logins.GetAllAsync();
                var userBankAccount = (await _repository.BankAccount.GetAllAsync()).FirstOrDefault(bc => bc.AccountNumber == user.AccountNumber);
                return View(new ConsultantViewModel
                {
                    SelectedUser = user,
                    loginSessions = allLogins.Where(u => u.UserEmail == email).OrderBy(l => l.TimeStamp)
                });
            }
            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ViewReviews()
        {
            var allReviews = await _repository.Review.GetAllAsync();
            return View(allReviews);
        }
        public async Task<IActionResult> DepositWithdraw(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return View(new ConsultantDepositModel
                {
                    AccountNumber = user.AccountNumber,
                    UserEmail = user.Email,
                });
            }
            return RedirectToAction("Index", "Consultant");
        }

        [HttpPost]
        public async Task<IActionResult> DepositWithdraw(ConsultantDepositModel model, string action)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UserEmail);
                if (user != null)
                {
                    var AllBankAcc = await _repository.BankAccount.GetAllAsync();
                    var userBankAcc = AllBankAcc.FirstOrDefault(bc => bc.UserEmail == user.Email);
                    if (userBankAcc != null)
                    {
                        if (action.ToLower() == "deposit")
                        {
                            userBankAcc.Balance += model.Amount;
                        }
                        else
                        {
                            if (userBankAcc.Balance - model.Amount < -50)
                            {
                                ModelState.AddModelError("", "User has insuffecient balance in their account");
                                return View(model);
                            }
                            userBankAcc.Balance -= model.Amount;
                        }
                        await _repository.BankAccount.UpdateAsync(userBankAcc);
                        var transaction = new Transaction
                        {
                            Amount = model.Amount,
                            UserEmail = model.UserEmail,
                            Reference = $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(action)} cash in account",
                            BankAccountIdReceiver = int.Parse(user.AccountNumber),
                            BankAccountIdSender = 0,
                            TransactionDate = DateTime.Now
                        };
                        await _repository.Transactions.AddAsync(transaction);
                        _repository.SaveChanges();
                        Message = $"Money Successfully {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(action)} to account";
                        return RedirectToAction("Index", "Consultant");
                    }
                    else
                    {
                        Message = "Couldn't find bank account, please contact system administrator";
                        ModelState.AddModelError("", "Couldn't find bank account");
                    }
                }
                else
                {
                    Message = "Couldn't find user, please contact system administrator";
                    ModelState.AddModelError("", "Couldn't find user");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ConsultantDeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var results = await _userManager.DeleteAsync(user);
                if (results.Succeeded)
                {
                    return RedirectToAction("Index", "Consultant");
                }
                return View();
            }
            return View();
        }
        public async Task<IActionResult> ConsultantUpdateUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return View(new ConsultantUpdateUserModel
                {
                    AccountNumber = user.AccountNumber,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    IDNumber = user.IDnumber,
                    Lastname = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConsultantUpdateUser(ConsultantUpdateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.LastName = model.Lastname;
                    user.DateOfBirth = model.DateOfBirth;
                    var result = await _userManager.UpdateAsync(user);
                    Message = "Updated User Details\n";
                    if (result.Succeeded)
                    {
                        if (model.Password != null && model.ConfirmPassword != null && model.Password == model.ConfirmPassword)
                        {
                            var passResults = await _userManager.RemovePasswordAsync(user);
                            if (passResults.Succeeded)
                            {
                                if ((await _userManager.AddPasswordAsync(user, model.Password)).Succeeded)
                                {
                                    Message += "Successfully updated password";
                                }
                                else
                                {
                                    Message += "Error updating password...Skipping process";
                                }
                            }
                        }
                        return RedirectToAction("Index", "Consultant");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Could not find user, please contact system admin");
                    Message = "Could not find user, please contact system admin";
                    return View(model);
                }
            }
            return View(model);
        }


        [HttpGet]
      
        public async Task<IActionResult> GenerateReports()
        {
            try
            {
                List<string> data = new List<string>();
                var reportContent = $"Banking Application\n{DateTime.Now:yyyyMMddHHmmss}\n\n" +
                    $"***Users\\Clients***\n" +
                    $"=====================\n" +
                    $"Account No\tFirst Name\tLast Name\tEmail Address\tStudent Number\n\n";
                var report = _userManager.Users;
                foreach (var u in report)
                {
                    if (await _userManager.IsInRoleAsync(u, "User"))
                    {
                        data.Add($"{u.AccountNumber}\t{u.FirstName}\t{u.LastName}\t{u.Email}\t{u.StudentStaffNumber}\n");
                    }
                }
                reportContent += string.Join('\n', data.ToArray());


                reportContent += $"\n***All Transactions***\n" +
                                 $"==========================\n" +
                    $"Account No\tFirst Name\tLast Name\tEmail Address\tStudent Number\n\n";
                var transactions = await _repository.Transactions.GetAllAsync();
                reportContent += string.Join('\n', transactions.Select(u => $"{u.UserEmail}\t{u.Amount}\t{u.BankAccountIdReceiver}\t{u.BankAccountIdSender}\n").ToArray());

                var contentBytes = Encoding.UTF8.GetBytes(reportContent);
                var fileName = $"Report_{DateTime.Now:yyyyMMddHHmmss}.txt";

                return File(contentBytes, "text/plain", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating report: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllUserReviews()
        {
            try
            {
                // Retrieve all reviews from the database
                var allReviews = await _repository.Review.GetAllAsync();

                // Pass the reviews to the view
                return View(allReviews);
            }
            catch (Exception ex)
            {
                // Handle any potential errors and show an error message
                Message = $"Error retrieving reviews: {ex.Message}";
                return View("Error");
            }
        }

        // Manage Customers
        public async Task<IActionResult> ManageCustomers()
        {
            var customers = await _repository.AppUser.FindByConditionAsync(u => u.IsCustomer);
            return View(customers);
        }

        // Edit Customer Profile (GET)
        public async Task<IActionResult> EditCustomer(string id)
        {
            var customer = await _repository.AppUser.FindByIdAsync(int.Parse(id));
            if (customer == null) return NotFound();

            var model = new EditUserViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                StudentNumber = (int?)customer.StudentStaffNumber,
                EmployeeNumber = (int?)customer.StudentStaffNumber,
                IDNumber = customer.IDnumber
            };

            return View(model);
        }

        // Edit Customer Profile (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _repository.AppUser.FindByIdAsync(int.Parse(model.Id));
                if (customer == null) return NotFound();

                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Email = model.Email;
                customer.StudentStaffNumber = (int)model.StudentNumber;
                customer.StudentStaffNumber = (int)model.EmployeeNumber;
                customer.IDnumber = (int)model.IDNumber;

                await _repository.AppUser.UpdateAsync(customer);
                _repository.SaveChanges();

                return RedirectToAction("ManageCustomers");
            }

            return View(model);
        }

        // Delete Customer (GET)
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var user = await _repository.AppUser.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // Delete Customer (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerConfirmed(string id)
        {
            var user = await _repository.AppUser.FindByIdAsync(int.Parse(id));
            if (user == null) return NotFound();

            await _repository.AppUser.DeleteAsync(int.Parse(id));
            _repository.SaveChanges();

            return RedirectToAction("ManageCustomers");
        }

        // Change Password for a User
        public IActionResult ChangePassword(string id)
        {
            var model = new ChangePasswordViewModel { UserId = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _repository.AppUser.FindByIdAsync(int.Parse(model.UserId));
                if (user == null) return NotFound();

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                    return RedirectToAction("ManageCustomers");

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // Assist Transactions (GET)
        public async Task<IActionResult> Transactions()
        {
            var customersWithAccounts = await _repository.AppUser.GetAllUsersAndBankAccount();
            return View(customersWithAccounts);
        }

        // Assist Transactions (POST) - Example for deposit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(int accountId, decimal amount)
        {
            var account = await _repository.BankAccount.FindByIdAsync(accountId);
            if (account == null) return NotFound();

            account.Balance += amount; // Update balance logic
            await _repository.BankAccount.UpdateAsync(account);

            var transaction = new Transaction
            {
                AccountID = account.Id,
                Amount = amount,
                transactionType = TransactionType.Deposit,
                TransactionDate = DateTime.Now,
                Description = "Deposit",
                BalanceAfter = account.Balance
            };

            await _repository.Transactions.AddAsync(transaction);
            _repository.SaveChanges();

            return RedirectToAction("Transactions");
        }


    }
}