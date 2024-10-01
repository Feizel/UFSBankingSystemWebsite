using UFSBankingSystemWebsite.Models;
using UFSBankingSystemWebsite.Models.ViewModels;
using UFSBankingSystemWebsite.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using Microsoft.IdentityModel.Abstractions;
using UFSBankingSystemWebsite.Data.SeedData;
using UFSBankingSystemWebsite.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using UFSBankingSystemWebsite.Data;

namespace UFSBankingSystemWebsite.Controllers
{
    [Authorize(Roles = "Admin, Consultant, FinancialAdvisor")]
    public class AdminDashboardController : Controller
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly string role = "User";

        public AdminDashboardController(IRepositoryWrapper wrapper, UserManager<User> userManager, AppDbContext context)
        {
            _wrapper = wrapper;
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

            // Retrieve all transactions from the repository
            var transactions = await _wrapper.Transactions.GetAllAsync();

            // Add sample transactions for demonstration
            transactions.AddRange(SampleData.SampleTransactions);

            // Retrieve users by role
            var consultants = (await _userManager.GetUsersInRoleAsync("Consultant")).ToList();
            var finAdvisors = (await _userManager.GetUsersInRoleAsync("FinancialAdvisor")).ToList();
            var users = (await _userManager.GetUsersInRoleAsync("User")).ToList();

            // Calculate total users
            int totalUsersCount = users.Count;

            // Determine active transactions 
            var activeTransactions = transactions.ToList(); // Retrieve all transactions

            // Create the view model with all necessary data
            var indexPageViewModel = new AdminViewModel()
            {
                CurrentPage = "index",
                Transactions = transactions,
                FinancialAdvisors = finAdvisors,
                Consultants = consultants,

                Users = users, // Count all users including samples
                Notifications = SampleData.SampleNotifications, // Use sample notifications
                ActiveTransactions = activeTransactions // Set active transactions here
            };

            indexPageViewModel.TotalUsers = totalUsersCount; // Set total user count
            return View(indexPageViewModel);
        }

        // MANAGE USERS
        [HttpGet]
        public async Task<IActionResult> UserManagement()
        {


            var users = await _wrapper.AppUser.GetAllUsersAndBankAccountAsync();
            var userPageViewModel = new UserPageViewModel()
            {
                AppUsers = users
            };

            return View(userPageViewModel);
        }

        // MANAGE INDIVIDUAL USER
        public async Task<IActionResult> ManageUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                // Handle the case where email is null or empty
                return BadRequest("Email cannot be null or empty.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return View(new ConsultantUpdateUserModel
                {
                    AccountNumber = user.AccountNumber,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    IDNumber = user.IDnumber,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AdminEditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IDNumber = user.IDnumber,
                AccountNumber = await _context.BankAccounts
                    .Where(b => b.Id == user.Id)
                    .Select(b => b.AccountNumber)
                    .FirstOrDefaultAsync(),
                IsActive = user.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update user properties
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.IDnumber = model.IDNumber;

                // Update user's active status if needed
                user.IsActive = model.IsActive;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Message"] = "User updated successfully!";
                    return RedirectToAction("UserManagement");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AdminViewUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Assuming you have a method to get the bank account associated with the user
            var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.Id == user.Id);

            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IDNumber = user.IDnumber,
                AccountNumber = bankAccount?.AccountNumber, // Use null-conditional operator
                Balance = bankAccount?.Balance ?? 0, // Default to 0 if no account found
                IsActive = user.IsActive
            };

            return View(model);
        }

        // DELETE USER
        public async Task<IActionResult> AdminDeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var results = await _userManager.DeleteAsync(user);
                if (results.Succeeded)
                {
                    return RedirectToAction("UserManagement", "AdminDashboard");
                }
                return View();
            }

            TempData["Message"] = "User deleted successful!";

            return View();
        }

        // CREATE USER
        [HttpGet]
        public IActionResult AdminCreateUser()
        {
            return View();
        }

        // CREATE USER
        [HttpPost]
        public async Task<IActionResult> AdminCreateUser(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create new user
            var user = new User
            {
                UserName = model.EmailAddress, // Set this to email for consistency
                Email = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IDnumber = model.IdPassportNumber,
                StudentStaffNumber = model.StudentStaffNumber,
                UserRole = "User", // Default to User role
                IsActive = true // Set initial status as active
            };

            // Create user in Identity
            var result = await _userManager.CreateAsync(user, model.Password);
            await _wrapper.SaveChangesAsync();

            // Generate random account number
            Random rndAccount = new Random();
            string _randomAccount = string.Empty;
            do
            {
                _randomAccount = rndAccount.Next(99999999, 999999999).ToString();
            }
            while (_userManager.Users.Where(u => u.AccountNumber != _randomAccount).FirstOrDefault() == null);
            user.AccountNumber = _randomAccount;

            if (result.Succeeded)
            {
                // Assign role if needed
                await _userManager.AddToRoleAsync(user, model.RegisterAs);

                // Create and add bank account
                BankAccount bankAccountMain = new()
                {
                    Id = user.Id, // Set the Id to link with the user
                    AccountNumber = _randomAccount,
                    Balance = 100m,
                    BankAccountType = "Savings",
                    AccountOrder = 1,
                    UserEmail = user.Email // Ensure this is set correctly as well
                };

                await _wrapper.BankAccount.AddAsync(bankAccountMain); // Add bank account to DB
                                                                      // Save changes for bank account creation
                await _wrapper.SaveChangesAsync();

                // Log initial transaction
                Transactions transaction = new Transactions
                {
                    BankAccountIdReceiver = int.Parse(_randomAccount), // Ensure this is valid
                    Amount = 100m,
                    Reference = "Fee for opening new account",
                    UserEmail = user.Email,
                    TransactionDate = DateTime.Now,
                    TransactionType = TransactionType.Deposit, // Define this enum appropriately
                    BalanceAfter = bankAccountMain.Balance // Set this to reflect balance after transaction
                };

                await _wrapper.Transactions.AddAsync(transaction); // Add transaction to DB
                await _wrapper.SaveChangesAsync();

                // Send notification with password
                var notification = new Notification
                {
                    Message = $"New account created. Email: {model.EmailAddress}, Password: {model.Password}",
                    UserEmail = model.EmailAddress,
                    NotificationDate = DateTime.Now,
                    IsRead = false,
                    Id = user.Id // Link notification to user ID
                };

                await _wrapper.Notification.AddAsync(notification); // Add notification to DB

                await _context.SaveChangesAsync(); // Save all changes at once

                TempData["Message"] = "User created successfully!";
                return RedirectToAction("UserManagement");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> AdminCreateUser(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Create the user
        //        var user = new User
        //        {
        //            UserName = model.EmailAddress,
        //            Email = model.EmailAddress,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            IDnumber = model.IdPassportNumber.ToString(),
        //            StudentStaffNumber = model.StudentStaffNumber.ToString(),
        //            UserRole = model.RegisterAs,
        //            IsActive = true
        //        };

        //        // Generate random account number
        //        Random rndAccount = new Random();
        //        string _randomAccount = string.Empty;
        //        do
        //        {
        //            _randomAccount = rndAccount.Next(99999999, 999999999).ToString();
        //        }
        //        while (_userManager.Users.Where(u => u.AccountNumber != _randomAccount).FirstOrDefault() == null);
        //        user.AccountNumber = _randomAccount;

        //        // Create user in Identity
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await _userManager.AddToRoleAsync(user, role);

        //            // Create and add bank account
        //            BankAccount bankAccountMain = new()
        //            {
        //                AccountNumber = _randomAccount,
        //                Balance = 100m,
        //                BankAccountType = "Savings",
        //                AccountOrder = 1,
        //                UserEmail = user.Email
        //            };
        //            await _wrapper.BankAccount.AddAsync(bankAccountMain);

        //            // Assuming you have a method to create a new user and this is part of that process

        //            // Log initial transaction
        //            try
        //            {
        //                // Ensure _randomAccount is a valid account ID
        //                if (int.TryParse(_randomAccount, out int bankAccountIdReceiver))
        //                {
        //                    // Create the transaction
        //                    Transactions transaction = new Transactions
        //                    {
        //                        BankAccountIdReceiver = bankAccountIdReceiver,
        //                        Amount = 100m,
        //                        Reference = "Fee for opening new account",
        //                        UserEmail = user.Email,
        //                        TransactionDate = DateTime.Now,
        //                        TransactionType = TransactionType.Deposit, // Ensure this enum value exists
        //                        BalanceAfter = 0m // Set this to the appropriate balance after the transaction
        //                    };

        //                    // Add the transaction to the database
        //                    await _wrapper.Transactions.AddAsync(transaction);
        //                }
        //                else
        //                {
        //                    throw new Exception("Invalid bank account ID.");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle any exceptions (log them or display an error message)
        //                Console.WriteLine($"Error creating transaction: {ex.Message}");
        //            }

        //            // Send notification with password
        //            try
        //            {
        //                var notification = new Notification
        //                {
        //                    Message = $"New account created. Email: {model.EmailAddress}, Password: {model.Password}",
        //                    UserEmail = model.EmailAddress,
        //                    NotificationDate = DateTime.Now,
        //                    IsRead = false,
        //                    Id = user.Id // Ensure this links to the correct user ID
        //                };

        //                // Add the notification to the database
        //                await _wrapper.Notification.AddAsync(notification);
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle any exceptions (log them or display an error message)
        //                Console.WriteLine($"Error sending notification: {ex.Message}");
        //            }

        //            // Save changes to the database after adding both entities
        //            await _context.SaveChangesAsync();
        //            _wrapper.SaveChanges();

        //            return RedirectToAction("Index", "AdminDashboard");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }

        //    return View(model);
        //}

        // MANAGE CONSULTANTS
        [HttpGet]
        public async Task<IActionResult> ManageConsultants()
        {
            var users = (await _userManager.GetUsersInRoleAsync("Consultant")).ToList();
            return View(users);
        }

        // EDIT CONSULTANT
        [HttpGet]
        public async Task<IActionResult> EditConsultant(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ConsultantUpdateUserModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IDNumber = user.IDnumber,
                AccountNumber = user.AccountNumber,
                PhoneNumber = user.PhoneNumber, // Assuming this property exists in User
                Userrole = user.UserRole
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditConsultant(ConsultantUpdateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return NotFound();
                }

                // Update user details
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber; // Assuming this property exists in User
                user.IDnumber = model.IDNumber;

                // Update password if provided
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    TempData["Message"] = "Consultant updated successfully!";
                    return RedirectToAction("ManageConsultants");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        // VIEW ADVICES
        [HttpGet]
        public async Task<IActionResult> ViewAdvices()
        {
            var allAdvices = await _wrapper.Notification.GetAllAsync();
            return View(allAdvices.Where(n => n.Message.StartsWith(" ")));
        }

        // ASSIGN ROLE
        [HttpGet]
        public async Task<IActionResult> AssignRole(string role, string email)
        {
            if (role != null && email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (await _userManager.IsInRoleAsync(user, "User"))
                    await _userManager.RemoveFromRoleAsync(user, "User");

                string Role = (role == "c") ? "Consultant" : "FinancialAdvisor";
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

        // Delete transaction
        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _wrapper.Transactions.RemoveAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllLogins(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var allLogins = await _wrapper.Logins.GetAllAsync();
              
                return View(allLogins);
        }

        // DEPOSIT / WITHDRAW
        [HttpGet]
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

        // DEPOSIT / WITHDRAW
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
                        await _wrapper.SaveChangesAsync();
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

        // GENERATE REPORT
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
                    user.LastName = model.LastName;
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

        // USER REVIEWS
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

        // ALL ADVICE
        public async Task<IActionResult> ViewAllAdvice()
        {
            // Retrieve all advice notifications
            var allAdvice = await _wrapper.Notification.GetAllAsync();

            // Filter for advice messages (assuming they start with "[ADVICE]")
            var adviceList = allAdvice.Where(n => n.Message.StartsWith(" ")).ToList();

            return View(adviceList);
        }

        // Toggle Status
        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(string id, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = isActive; // Toggle the active status
            await _userManager.UpdateAsync(user);

            TempData["Message"] = $"User {user.UserName} has been {(isActive ? "activated" : "deactivated")}.";
            return RedirectToAction("ManageUsers");
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            // Fetch the user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Create a view model to hold user information and roles
            var model = new AssignRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _roleManager.Roles.ToListAsync() // Get all available roles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Remove existing roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                // Assign new role(s)
                if (model.SelectedRoles != null && model.SelectedRoles.Any())
                {
                    await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    TempData["Message"] = "Roles assigned successfully!";
                    return RedirectToAction("ManageUsers"); // Redirect to a suitable action
                }

                TempData["Message"] = "No roles were selected.";
            }

            return View(model); // If we got this far, something failed, redisplay form
        }
    }
}
