﻿using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels;
using UFSBankingSystem.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using Microsoft.IdentityModel.Abstractions;

namespace UFSBankingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly string role = "User";

        public AdminController(IRepositoryWrapper wrapper, UserManager<AppUser> userManager)
        {
            _wrapper = wrapper;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string currentPage = "index")
        {
            var transactions = await _wrapper.Transactions.GetAllAsync();
            var consultants = (await _userManager.GetUsersInRoleAsync("Consultant")).ToList();
            var finadvisors = (await _userManager.GetUsersInRoleAsync("FAdvisor")).ToList();
            var users = (await _userManager.GetUsersInRoleAsync("User")).ToList();

            var indexPageViewModel = new IndexPageViewModel()
            {
                CurrentPage = currentPage,
                Transactions = transactions,
                FinAdvisor = finadvisors,
                Consultants = consultants,
                Users = users
            };

            return View(indexPageViewModel);
        }
        public async Task<IActionResult> ViewCustomer()
        {
            List<AppUser> lstUsers = new List<AppUser>();
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

        [HttpGet]
        public async Task<IActionResult> Users()
        {


            var users = await _wrapper.AppUser.GetAllUsersAndBankAccount();
            var userPageViewModel = new UserPageViewModel()
            {
                AppUsers = users
            };

            return View(userPageViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Consultants()
        {
            var users = (await _userManager.GetUsersInRoleAsync("Consultant")).ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAdvices()
        {
            var allAdvices = await _wrapper.Notification.GetAllAsync();
            return View(allAdvices.Where(n => n.Message.StartsWith(" ")));
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(string role, string email)
        {
            if (role != null && email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (await _userManager.IsInRoleAsync(user, "User"))
                    await _userManager.RemoveFromRoleAsync(user, "User");

                string Role = (role == "c") ? "Consultant" : "FinAdvisor";
                var result = await _userManager.AddToRoleAsync(user, Role);
                if (result.Succeeded)
                {
                    Message = $"User successfully assigned to {Role}";
                    return RedirectToAction("Index", "Admin");
                }
            }
            Message = $"Failed to assign user to role";
            return RedirectToAction("Index", "Admin");
        }

        //delete transaction
        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _wrapper.Transactions.RemoveAsync(id);
            return RedirectToAction("Index");
        }

        [TempData]
        public string Message { get; set; }

        [HttpGet]
        public async Task<IActionResult> ViewAllLogins(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var allLogins = await _wrapper.Logins.GetAllAsync();
              
                return View(allLogins);
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
                    var AllBankAcc = await _wrapper.BankAccount.GetAllAsync();
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
                        await _wrapper.BankAccount.UpdateAsync(userBankAcc);
                        var transaction = new Transactions
                        {
                            Amount = model.Amount,
                            UserEmail = model.UserEmail,
                            Reference = $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(action)} cash in account",
                            BankAccountIdReceiver = int.Parse(user.AccountNumber),
                            BankAccountIdSender = 0,
                            TransactionDate = DateTime.Now
                        };
                        await _wrapper.Transactions.AddAsync(transaction);
                        _wrapper.SaveChanges();
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

        [HttpPost]
        public IActionResult ChangePassword()
        {
            return View();
        }

        public async Task<IActionResult> AdminDeleteUser(string email)
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
        public async Task<IActionResult> ManageUser(string email)
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

        [HttpGet]
        public async Task<IActionResult> GenerateReport()
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
                var transactions = await _wrapper.Transactions.GetAllAsync();
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

        [HttpPost]
        public async Task<IActionResult> ManageUser(ConsultantUpdateUserModel model)
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
        public async Task<IActionResult> ViewAllUserReviews()
        {
            try
            {
                // Retrieve all reviews from the database
                var allReviews = await _wrapper.Review.GetAllAsync();

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

        public async Task<IActionResult> ViewAllAdvice()
        {
            // Retrieve all advice notifications
            var allAdvice = await _wrapper.Notification.GetAllAsync();

            // Filter for advice messages (assuming they start with "[ADVICE]")
            var adviceList = allAdvice.Where(n => n.Message.StartsWith(" ")).ToList();

            return View(adviceList);
        }



        [HttpGet]
        public IActionResult CreateUser(string registerAs = "student")
        {
            return View(new RegisterViewModel() { RegisterAs = registerAs });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create the user
                var user = new AppUser
                {
                    Email = model.EmailAddress,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IDnumber = model.IdPassportNumber,
                    StudentStaffNumber = model.StudentStaffNumber,
                    UserName = (model.LastName + model.FirstName).Substring(0, 10),
                    UserRole = model.RegisterAs
                };

                // Generate random account number
                Random rndAccount = new Random();
                string _randomAccount = string.Empty;
                do
                {
                    _randomAccount = rndAccount.Next(99999999, 999999999).ToString();
                }
                while (_userManager.Users.Where(u => u.AccountNumber != _randomAccount).FirstOrDefault() == null);
                user.AccountNumber = _randomAccount;

                // Create user in Identity
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);

                    // Create and add bank account
                    BankAccount bankAccountMain = new()
                    {
                        AccountNumber = _randomAccount,
                        Balance = 100m,
                        BankAccountType = "Savings",
                        AccountOrder = 1,
                        UserEmail = user.Email
                    };
                    await _wrapper.BankAccount.AddAsync(bankAccountMain);

                    // Log initial transaction
                    Transactions transaction = new()
                    {
                        BankAccountIdReceiver = int.Parse(_randomAccount),
                        Amount = 100m,
                        Reference = "fee Open new account",
                        UserEmail = user.Email,
                        TransactionDate = DateTime.Now
                    };
                    await _wrapper.Transactions.AddAsync(transaction);

                    // Send notification with password
                    var notification = new Notification
                    {
                        Message = $"New account created. Email: {model.EmailAddress}, Password: {model.Password}",
                        UserEmail = model.EmailAddress,
                        NotificationDate = DateTime.Now,
                        IsRead = false
                    };
                    await _wrapper.Notification.AddAsync(notification);
                    _wrapper.SaveChanges();

                    return RedirectToAction("Index", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}
