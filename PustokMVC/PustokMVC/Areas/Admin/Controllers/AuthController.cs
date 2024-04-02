using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokMVC.Areas.Admin.ViewModels;
using PustokMVC.Models;

namespace PustokMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser admin = null;

            admin = await _userManager.FindByNameAsync(adminVM.Username);

            if (admin is null)
            {
                ModelState.AddModelError("", "Invalid credentials!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(admin, adminVM.Password,false,false);

            if(!result.Succeeded) 
            {
                ModelState.AddModelError("", "Invalid credentials!");
                return View();
            }

            return RedirectToAction("index","dashboard");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);

            if(user is not null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetTokenLink = Url.Action("ResetPassword", "Auth", new { email = forgotPasswordVM.Email, token = token }, Request.Scheme);

                // email service...

                return View("ConfirmPage");
            }
            else
            {
                ModelState.AddModelError("Email", "User not found");
                return View();
            }

        }

        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null) return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if(user is not null)
            {
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Token, resetPasswordViewModel.NewPassword);

                if(!result.Succeeded)
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                        return View();
                    }
                }
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
