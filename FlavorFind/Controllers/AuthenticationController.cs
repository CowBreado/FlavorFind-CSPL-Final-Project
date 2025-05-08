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
    }
}
