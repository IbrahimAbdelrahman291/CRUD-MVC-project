using DemoDAL.Models;
using DemoPL.Helpers;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
        //register
        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Regiser(RegisterViewModel model)
        {
            if (ModelState.IsValid)//server side validation
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };
                var Result = await _userManager.CreateAsync(User,model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty,error.Description);
                    }
                }
            }
            return View(model);
        }
        //login
        public IActionResult Login() 
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var Result = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (Result)
                    {
                        //login
                        var LogedResult = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe,false);
                        if (LogedResult.Succeeded)
                        {
                            return RedirectToAction("Index","Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,"Password is incorrect");
                    }
                }
                else
                {
					ModelState.AddModelError(string.Empty, "Email is not exist");
				}
            }
            return View(model);
        }
        //signout
        public new async Task<IActionResult> SignOut() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        //forget password
        public IActionResult ForgetPassword() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model) 
        {
            if (ModelState.IsValid) 
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var Token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    //https://localhost:5001/Account/ResetPassword?email=ibrahimabdelrahman2912003@gmail.com&Token=sadhjasdasda
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email,token = Token},Request.Scheme);
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset password",
                        Body = ResetPasswordLink
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                else
                {
					ModelState.AddModelError(string.Empty, "Email is not exist");
				}
            }
            
            return View("ForgetPassword", model);
            
        }
        public IActionResult CheckYourInbox() 
        {
            return View();
        }
        //reset password
        public IActionResult ResetPassword(string email,string token) 
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
            if (ModelState.IsValid) 
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var User = await _userManager.FindByEmailAsync(email);
                var Result = await _userManager.ResetPasswordAsync(User,token,model.Password);
                if (Result.Succeeded) 
                {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }
            return View(model);
		}
	}
}
