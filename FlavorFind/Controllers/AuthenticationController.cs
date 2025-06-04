using CS2ARonaldAbel_MVCPROJECT.Models;
using FlavorFind.BusLogic.Model; // For UserRegistrationModel
using FlavorFind.BusLogic.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks; // For async Task
using System.ComponentModel.DataAnnotations;

public class UserRegistrationModel
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}


namespace FlavorFind.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserService _userService = new UserService();

        public IActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            // Your existing simple login can be replaced or augmented
            // if (email == "admin@example.com" && password == "1234") { ... }

            var user = _userService.LoginUser(email, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim() == "" ? user.Email : $"{user.FirstName} {user.LastName}".Trim())
                    // Add other claims as needed
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true /* For "Remember Me" */ };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Recipe"); // Default redirect after login
            }

            ViewBag.Error = "Invalid email or password.";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userService.RegisterUser(model.Email, model.Password, model.FirstName, model.LastName);
                    if (user != null)
                    {
                        TempData["SuccessMessage"] = "Registration successful! Please log in.";
                        return RedirectToAction("Login");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message; // e.g., "User with this email already exists."
                }
            }
            return View(model); // Show errors if any
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Welcome", "Authentication");
        }
        
        public IActionResult AccessDenied()
        {
            return View(); // Create an AccessDenied.cshtml view
        }
    }
}
