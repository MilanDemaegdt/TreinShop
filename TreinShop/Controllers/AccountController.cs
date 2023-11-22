using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TreinShop.Areas.Identity.Data;
using TreinShop.Areas.Identity.Pages.Account;
using TreinShop.Util.Mail;

namespace TreinShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSend _emailSender;

        public AccountController(UserManager<User> userManager, IOptions<EmailSettings> emailSettings)
        {
            _userManager = userManager;
            _emailSender = new EmailSend(emailSettings);
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Forgot Password
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Generate password reset token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Construct the reset link URL with the token
                    var resetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    // Compose the email message
                    var subject = "Reset Your Password";
                    var message = $"Please click the following link to reset your password: <a href='{resetLink}'>Reset Password</a>";

                    // Send the password reset email
                    await _emailSender.SendEmailAsync(model.Email, subject, message);

                    ViewBag.EmailSent = true;
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordVM { Email = email, Token = token };
            ViewBag.ResetSuccess = false;
            return View(model);
        }

        // POST: Reset Password
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        ViewBag.ResetSuccess = true;
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid user.");
                }
            }

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
