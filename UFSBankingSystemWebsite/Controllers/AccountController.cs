using UFSBankingSystem.Data.Interfaces;
using UFSBankingSystem.Models;
using UFSBankingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace UFSBankingSystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRepositoryWrapper wrapper;
        private readonly string role = "User";

        public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager,
            RoleManager<IdentityRole> _roleManager, IRepositoryWrapper _wrapper)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            wrapper = _wrapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string registerAs = "student")
        {
            return View(new RegisterViewModel() { RegisterAs = registerAs });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new(role));

                User user = new()
                {
                    UserName = (registerModel.LastName + registerModel.FirstName).Substring(0, 10),
                    IDnumber = registerModel.IdPassportNumber,
                    Email = registerModel.EmailAddress,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    StudentStaffNumber = registerModel.StudentStaffNumber,
                    UserRole = registerModel.RegisterAs
                };

                Random rndAccount = new Random();
                string _randomAccount = string.Empty;
                do
                {
                    _randomAccount = rndAccount.Next(99999999, 999999999).ToString();
                }
                while (userManager.Users.Where(u => u.AccountNumber != _randomAccount).FirstOrDefault() == null);
                user.AccountNumber = _randomAccount;



                IdentityResult result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                    BankAccount bankAccountMain = new()
                    {
                        AccountNumber = _randomAccount,
                        Balance = 100m,
                        BankAccountType = "Savings",
                        AccountOrder = 1,
                        UserEmail = user.Email,
                    };
                    await wrapper.BankAccount.AddAsync(bankAccountMain);
                    Transactions transaction = new()
                    {
                        BankAccountIdReceiver = int.Parse(_randomAccount),
                        Amount = 100m,
                        Reference = "fee Open new account ",
                        UserEmail = user.Email,
                        TransactionDate = DateTime.Now,

                    };
                    await wrapper.Transactions.AddAsync(transaction);
                    var signin_result = await signInManager.PasswordSignInAsync(user, registerModel.Password,
                        isPersistent: false, lockoutOnFailure: false);
                    if (signin_result.Succeeded)
                    {
                        var newLogin = new LoginSessions
                        {
                            TimeStamp = DateTime.Now,
                            UserEmail = user.Email,
                        };
                        await wrapper.Logins.AddAsync(newLogin);
                        wrapper.SaveChanges();

                        if (await userManager.IsInRoleAsync(user, "Consultant"))
                            return RedirectToAction("Index", "Consultant");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    foreach (var error in result.Errors.Select(e => e.Description))
                        ModelState.AddModelError("", error);
            }
            return View(registerModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> UpdateProfile()
        //{
        //    var username = User.Identity.Name;

        //    var user = await userManager.FindByNameAsync(username);
        //    var model = new UpdateProfileViewModel
        //    {
        //        Email = user.Email,
        //        PhoneNumber = user.PhoneNumber,

        //        IDNumber = user.IDnumber,

        //        Userrole = user.UserRole,
        //        Lastname = user.LastName + " " + user.FirstName,

        //    };
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByNameAsync(User.Identity.Name);
        //        if (user != null)
        //        {
        //            user.Email = model.Email;
        //            user.PhoneNumber = model.PhoneNumber;
        //            var result = await userManager.UpdateAsync(user);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else
        //            {
        //                foreach (var error in result.Errors)
        //                {
        //                    ModelState.AddModelError("", error.Description);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Could not find user, please contact system admin");
        //            return View(model);
        //        }
        //    }
        //    return View(model);
        //}

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
                Lastname = user.LastName + " " + user.FirstName,
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
                User user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync
                        (user, model.Password, isPersistent: model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var newLogin = new LoginSessions
                        {
                            TimeStamp = DateTime.Now,
                            UserEmail = user.Email,
                        };
                        await wrapper.Logins.AddAsync(newLogin);
                        wrapper.SaveChanges();

                        if (await userManager.IsInRoleAsync(user, "Consultant"))
                            return RedirectToAction("Index", "Consultant");
                        return Redirect(model?.ReturnUrl ?? "/Home/Index");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid email or password");
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