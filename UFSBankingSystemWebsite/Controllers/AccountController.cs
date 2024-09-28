using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System.Data;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystem.Controllers
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
                User user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Attempt to sign in the user
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // Set ViewBag.FirstName for use in the layout
                        ViewBag.FirstName = user.FirstName;
                        //ViewBag.LastName = user.LastName;

                        // Log the login session
                        var newLogin = new LoginSession
                        {
                            TimeStamp = DateTime.Now,
                            UserEmail = user.Email,
                        };
                        await _repoWrapper.Logins.AddAsync(newLogin);
                        _repoWrapper.SaveChanges(); // Ensure changes are saved

                        // Redirect based on user role
                        if (await userManager.IsInRoleAsync(user, _adminRole))
                            return RedirectToAction("Index", "AdminDashboard");
                        else if (await userManager.IsInRoleAsync(user, _consultantRole))
                            return RedirectToAction("Index", "ConsultantDashboard");
                        else if (await userManager.IsInRoleAsync(user, _financialAdvisorRole))
                            return RedirectToAction("Index", "FinancialAdvisorDashboard");
                        else if (await userManager.IsInRoleAsync(user, _customerRole))
                            return RedirectToAction("Index", "CustomerDashboard");

                        // Default redirect
                        //return Redirect(model?.ReturnUrl ?? "/Home/Index");
                    }
                }
            }

            // Add an error message if login fails
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string registerAs, string userType, string userRole)
        {
            return View(new RegisterViewModel() 
            { 
                RegisterAs = registerAs,
                UserType = userType,
                Role = userRole
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                // Ensure roles exist
                await EnsureRolesExist();

                // Create a new user
                User user = new()
                {
                    UserName = registerModel.EmailAddress, // Use email as username for clarity
                    IDnumber = registerModel.IdPassportNumber,
                    Email = registerModel.EmailAddress,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    StudentStaffNumber = registerModel.StudentStaffNumber
                    //UserRole = registerModel.Role,
                    //UserType = registerModel.UserType
                };

                // Create the user
                IdentityResult result = await userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    // Assign role to user as Customer
                    await userManager.AddToRoleAsync(user, _customerRole);

                    // Create a bank account for the user
                    BankAccount bankAccountMain = new()
                    {
                        AccountNumber = GenerateAccountNumber(), // Implement this method for unique account numbers
                        Balance = 100m, // Initial balance
                        BankAccountType = "Savings",
                        AccountOrder = 1,
                        UserEmail = user.Email, // Associate with the registered user's email
                        UserId = user.Id // Use Id from IdentityUser as foreign key
                    };

                    // Save the bank account to the database
                    await _repoWrapper.BankAccount.AddAsync(bankAccountMain);

                    // Create a transaction for initial deposit
                    Models.Transaction transaction = new()
                    {
                        BankAccountIdReceiver = bankAccountMain.BankAccountID, // Assuming this is how you reference accounts
                        Amount = 100m,
                        Reference = "Initial deposit",
                        UserEmail = user.Email,
                        TransactionDate = DateTime.Now,
                    };

                    await _repoWrapper.Transactions.AddAsync(transaction);

                    // Sign in the user after successful registration
                    var signin_result = await signInManager.PasswordSignInAsync(user, registerModel.Password, isPersistent: false, lockoutOnFailure: false);

                    if (signin_result.Succeeded)
                    {
                        var newLogin = new LoginSession
                        {
                            TimeStamp = DateTime.Now,
                            UserEmail = user.Email,
                        };

                        await _repoWrapper.Logins.AddAsync(newLogin);
                        _repoWrapper.SaveChanges(); // Ensure changes are saved

                        return RedirectToAction("Index", "CustomerDashboard");
                    }
                }
                else
                {
                    foreach (var error in result.Errors.Select(e => e.Description))
                        ModelState.AddModelError("", error);
                }
            }
            return View(registerModel);
        }

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