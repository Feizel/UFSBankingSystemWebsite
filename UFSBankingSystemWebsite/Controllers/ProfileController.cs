using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFSBankingSystemWebsite.Data;
using UFSBankingSystemWebsite.Models;
using UFSBankingSystemWebsite.Models.ViewModels.Admin;

namespace UFSBankingSystemWebsite.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<User> userManager;

        public ProfileController(AppDbContext appDbContext, UserManager<User>
            userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(string Message)
        {

            if (!string.IsNullOrEmpty(Message))
                ViewBag.Message = Message;
            return View(await userManager.FindByEmailAsync(User!.Identity!.Name));


        }
        //public async Task<IActionResult> Edit()
        //{

        //    var user = await userManager.FindByEmailAsync(User!.Identity!.Name);
        //    return View(new UserViewModel
        //    {
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //    });
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit(UserViewModel user)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View(user);
        //    }
        //    var _user = await userManager.FindByEmailAsync(User!.Identity!.Name);

        //    _user!.FirstName = user.FirstName;
        //    _user.LastName = user.LastName;

        //    appDbContext.Users.Update(_user);
        //    if (await appDbContext.SaveChangesAsync() > 0)
        //        return RedirectToAction(nameof(Index), new { Message = "Your profile details were updated successful." });
        //    return View(user);
        //}
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
                var user = await userManager.GetUserAsync(User) as User;

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.StudentStaffNumber = model.StudentNumber;
                user.StudentStaffNumber = model.EmployeeNumber;
                user.IDnumber = model.IDNumber;

                var result = await userManager.UpdateAsync(user);
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
        public IActionResult ChangePassword()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user = await userManager.FindByEmailAsync(User.Identity!.Name);
            IdentityResult results = await userManager.
                ChangePasswordAsync(user, model.CurrentPassword, model.CurrentPassword);
            if (results.Succeeded)
                return RedirectToAction(nameof(Index), new { Message = "Your password was updated successful." });

            foreach (var item in results.Errors)
                ModelState.AddModelError(item.Code, item.Description);

            return View(model);
        }
    }
}
