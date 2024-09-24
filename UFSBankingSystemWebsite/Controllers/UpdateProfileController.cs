using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UFSBankingSystem.Models.ViewModels;
using UFSBankingSystem.Models;

public class UpdateProfileController : Controller
{
    // Inject your DbContext or UserManager (depending on your data storage)
    private readonly UserManager<User> _userManager;

    public UpdateProfileController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProfile()
    {
        // Get the current user and pass their info to the view model
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new UpdateProfileViewModel
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IDNumber = user.Id, // Assuming ID is stored in the user object
            Userrole = user.UserRole, // Assuming you have a UserRole property
            Lastname = user.LastName
            
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Update the user's profile
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            // Save changes
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Redirect to some confirmation page or profile view
                return RedirectToAction("Profile", "Account");
            }

            // Add any errors to the ModelState if the update fails
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }
}
