using UFSBankingSystemWebsite.Models;
using UFSBankingSystemWebsite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System.Data;
using UFSBankingSystemWebsite.Data.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;

namespace UFSBankingSystemWebsite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly string _customerRole = "User";
        private readonly string _consultantRole = "Consultant";
        private readonly string _financialAdvisorRole = "FinancialAdvisor";
        private readonly string _adminRole = "Admin";

        public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager,
            RoleManager<IdentityRole> _roleManager, IRepositoryWrapper _wrapper)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            _repoWrapper = _wrapper;
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Try to sign in
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // Set user's name in ViewBag
                        ViewBag.FirstName = user.FirstName;

                        // Log the login
                        var newLogin = new LoginSession
                        {
                            TimeStamp = DateTime.Now,
                            UserEmail = user.Email,
                        };
                        await _repoWrapper.Logins.AddAsync(newLogin);
                        _repoWrapper.SaveChanges();

                        // Redirect based on role
                        if (await userManager.IsInRoleAsync(user, _adminRole))
                            return RedirectToAction("Index", "AdminDashboard");
                        else if (await userManager.IsInRoleAsync(user, _consultantRole))
                            return RedirectToAction("Index", "ConsultantDashboard");
                        else if (await userManager.IsInRoleAsync(user, _financialAdvisorRole))
                            return RedirectToAction("Index", "FinancialAdvisorDashboard");
                        else if (await userManager.IsInRoleAsync(user, _customerRole))
                            return RedirectToAction("Index", "CustomerDashboard");

                        // If no specific role, go to home
                        return Redirect(model?.ReturnUrl ?? "/Home/Index");
                    }
                }
            }

            // If we got this far, something failed
            ModelState.AddModelError("", "Login failed. Check your email and password.");
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string registerAs)
        {
            return View(new RegisterViewModel() 
            { 
                RegisterAs = registerAs
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Make sure the Customer role exists
                if (await roleManager.FindByNameAsync(_customerRole) == null)
                    await roleManager.CreateAsync(new IdentityRole(_customerRole));

                // Create the user
                var user = new User
                {
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress,
                    IDnumber = model.IdPassportNumber.ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    StudentStaffNumber = model.StudentStaffNumber.ToString(),
                    UserRole = "User" // Default to User role
                };

                // Generate a unique account number
                var accountNumber = "";
                var rnd = new Random();
                do
                {
                    accountNumber = rnd.Next(100000000, 999999999).ToString();
                } while (await userManager.Users.AnyAsync(u => u.AccountNumber == accountNumber));

                user.AccountNumber = accountNumber;

                // Try to create the user
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Add user to Customer role
                    await userManager.AddToRoleAsync(user, _customerRole);

                    // Create a bank account for the user
                    var bankAccount = new BankAccount
                    {
                        Id = user.Id,
                        AccountName = "Default",
                        AccountNumber = accountNumber,
                        Balance = 100m, // Starting balance
                        BankAccountType = "Savings",
                        AccountOrder = 1,
                        UserEmail = user.Email,
                    };
                    await _repoWrapper.BankAccount.AddAsync(bankAccount);
                    _repoWrapper.SaveChanges(); // Ensure it is saved

                    var bankAccountId = int.Parse(accountNumber); 
                    var existingAccount = await _repoWrapper.BankAccount.FindByIdAsync(bankAccountId);
                    if (existingAccount == null)
                    {
                        // Log or throw an error indicating that the account does not exist
                        throw new Exception("Bank account does not exist.");
                    }
                    else
                    {
                        // Proceed with creating a transaction if the account exists

                        // Log the initial deposit
                        var transaction = new Transactions
                        {
                            BankAccountIdReceiver = bankAccount.BankAccountID, // Use the ID from the created bank account
                            Amount = 100m,
                            Reference = "Initial deposit",
                            UserEmail = user.Email,
                            TransactionDate = DateTime.Now,
                        };
                        await _repoWrapper.Transactions.AddAsync(transaction);
                        _repoWrapper.SaveChanges(); // Ensure transaction is saved
                    }

                    // Save changes
                    _repoWrapper.SaveChanges();

                    // Sign in the new user
                    await signInManager.SignInAsync(user, isPersistent: false);

                    // Log the login
                    var loginSession = new LoginSession
                    {
                        TimeStamp = DateTime.Now,
                        UserEmail = user.Email,
                    };
                    await _repoWrapper.Logins.AddAsync(loginSession);
                    _repoWrapper.SaveChanges();

                    return RedirectToAction("Index", "CustomerDashboard");
                }

                // If we got this far, something failed
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel registerModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Ensure roles exist
        //        await EnsureRolesExist();

        //        // Create a new user
        //        User user = new()
        //        {
        //            UserName = registerModel.EmailAddress, // Use email as username for clarity
        //            IDnumber = registerModel.IdPassportNumber.ToString(),
        //            Email = registerModel.EmailAddress,
        //            FirstName = registerModel.FirstName,
        //            LastName = registerModel.LastName,
        //            StudentStaffNumber = registerModel.StudentStaffNumber.ToString()
        //        };

        //        // Create the user
        //        IdentityResult result = await userManager.CreateAsync(user, registerModel.Password);

        //        if (result.Succeeded)
        //        {
        //            // Assign role to user as Customer
        //            await userManager.AddToRoleAsync(user, _customerRole);

        //            // Create a bank account for the user
        //            BankAccount bankAccountMain = new()
        //            {
        //                AccountNumber = GenerateAccountNumber(), // Implement this method for unique account numbers
        //                Balance = 100m, // Initial balance
        //                BankAccountType = "Savings",
        //                AccountOrder = 1,
        //                UserEmail = user.Email, // Associate with the registered user's email
        //                UserId = user.Id // Use Id from IdentityUser as foreign key
        //            };

        //            // Save the bank account to the database
        //            await _repoWrapper.BankAccount.AddAsync(bankAccountMain);

        //            // Create a transaction for initial deposit
        //            Models.Transaction transaction = new()
        //            {
        //                BankAccountIdReceiver = bankAccountMain.BankAccountID, // Assuming this is how you reference accounts
        //                Amount = 100m,
        //                Reference = "Initial deposit",
        //                UserEmail = user.Email,
        //                TransactionDate = DateTime.Now,
        //            };

        //            await _repoWrapper.Transactions.AddAsync(transaction);

        //            // Sign in the user after successful registration
        //            var signin_result = await signInManager.PasswordSignInAsync(user, registerModel.Password, isPersistent: false, lockoutOnFailure: false);

        //            if (signin_result.Succeeded)
        //            {
        //                var newLogin = new LoginSession
        //                {
        //                    TimeStamp = DateTime.Now,
        //                    UserEmail = user.Email,
        //                };

        //                await _repoWrapper.Logins.AddAsync(newLogin);
        //                _repoWrapper.SaveChanges(); // Ensure changes are saved

        //                return RedirectToAction("Index", "CustomerDashboard");
        //            }
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors.Select(e => e.Description))
        //                ModelState.AddModelError("", error);
        //        }
        //    }
        //    ModelState.AddModelError("", "Registration failed");
        //    return View(registerModel);
        //}

        // Helper method to ensure roles exist
        private async Task EnsureRolesExist()
        {
            if (await roleManager.FindByNameAsync(_customerRole) == null)
                await roleManager.CreateAsync(new IdentityRole(_customerRole));
            if (await roleManager.FindByNameAsync(_adminRole) == null)
                await roleManager.CreateAsync(new IdentityRole(_adminRole));
            if (await roleManager.FindByNameAsync(_consultantRole) == null)
                await roleManager.CreateAsync(new IdentityRole(_consultantRole));
            if (await roleManager.FindByNameAsync(_financialAdvisorRole) == null)
                await roleManager.CreateAsync(new IdentityRole(_financialAdvisorRole));
        }
        private string GenerateAccountNumber()
        {
            // Logic to generate a unique account number
            return "ACCT" + new Random().Next(100000, 999999).ToString();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var username = User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new UpdateProfileViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IDNumber = user.IDnumber,
                LastName = user.LastName + " " + user.FirstName,
                Userrole = user.UserRole,
                AccountNumber = user.AccountNumber // Add the AccountNumber to the model
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
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
                    return View(model);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}