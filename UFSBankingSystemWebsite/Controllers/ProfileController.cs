//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using UFSBankingSystem.Data;
//using UFSBankingSystem.Models.Entities;
//using UFSBankingSystem.Models.ViewModel;

//namespace UFSBankingSystem.Controllers
//{
//    public class ProfileController : Controller
//    {
//        private readonly AppDbContext appDbContext;
//        private readonly UserManager<AppUser> userManager;

//        public ProfileController(AppDbContext appDbContext, UserManager<AppUser>
//            userManager)
//        {
//            this.appDbContext = appDbContext;
//            this.userManager = userManager;
//        }
//        public async Task<IActionResult> Index(string Message)
//        {
           
//            if (!string.IsNullOrEmpty(Message))
//                ViewBag.Message = Message;
//            return View(await userManager.FindByEmailAsync(User!.Identity!.Name));

            
//        }
//        public async Task<IActionResult> Edit()
//        {
           
//            var user = await userManager.FindByEmailAsync(User!.Identity!.Name);
//            return View(new AppUserViewModel
//            {
//                Name = user.Name,
//                Surname = user.Surname,
//                StudentNumber = user.Identity
//            });
//        }
//        [HttpPost]
//        public async Task<IActionResult> Edit(AppUserViewModel user)
//        {
           
//            if (!ModelState.IsValid)
//            {
//                return View(user);
//            }
//            var _user = await userManager.FindByEmailAsync(User!.Identity!.Name);

//            _user!.Name = user.Name;
//            _user.Surname = user.Surname;
//            _user.Identity = user.StudentNumber;

//            appDbContext.Users.Update(_user);
//            if (await appDbContext.SaveChangesAsync() > 0)
//                return RedirectToAction(nameof(Index), new { Message = "Your profile details were updated successful." });
//            return View(user);
//        }
//        public IActionResult ChangePassword()
//        {
           
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> ChangePassword(PasswordChangeModel model)
//        {
           
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            AppUser user = await userManager.FindByEmailAsync(User.Identity!.Name);
//            IdentityResult results = await userManager.
//                ChangePasswordAsync(user, model.CurrentPassword, model.Password);
//            if (results.Succeeded)
//                return RedirectToAction(nameof(Index), new { Message = "Your password was updated successful." });

//            foreach (var item in results.Errors)
//                ModelState.AddModelError(item.Code, item.Description);

//            return View(model);
//        }
//    }
//}
