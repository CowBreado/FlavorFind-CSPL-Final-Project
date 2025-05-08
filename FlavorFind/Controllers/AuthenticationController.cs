using CS2ARonaldAbel_MVCPROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlavorFind.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email == "admin@example.com" && password == "1234")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid credentials.";
            return View();
        }
    }
}
