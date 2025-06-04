using FlavorFind.BusLogic.Model;
using FlavorFind.BusLogic.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using System.Security.Claims; // Required for User.FindFirstValue

namespace FlavorFind.Controllers
{
    [Authorize] // This will require users to be logged in to access any action here
    public class RecipeController : Controller
    {
        private readonly RecipeService _recipeService = new RecipeService();

        // Helper to get current user's ID
        private int GetCurrentUserId()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdValue, out int userId))
            {
                return userId;
            }
            // This should ideally not happen if [Authorize] is working
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }

        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var recipes = _recipeService.GetRecipesByUserId(userId);
            return View(recipes);
        }

        public IActionResult Create()
        {
            return View(); // The view should not require UserId input from the form
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                // recipe.UserId will be set in the service
                _recipeService.AddRecipeForUser(recipe, userId);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        public IActionResult Edit(int id)
        {
            var userId = GetCurrentUserId();
            var recipe = _recipeService.GetRecipeByIdAndUserId(id, userId);
            if (recipe == null)
            {
                return NotFound(); // Or an access denied view
            }
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                // recipe.UserId will be set/verified in the service
                var success = _recipeService.UpdateRecipeForUser(recipe, userId);
                if (!success)
                {
                     return NotFound(); // Or an access denied view
                }
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        public IActionResult Delete(int id) // GET for confirmation page
        {
            var userId = GetCurrentUserId();
            var recipe = _recipeService.GetRecipeByIdAndUserId(id, userId);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe); // Pass recipe to view for confirmation
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            var success = _recipeService.DeleteRecipeForUser(id, userId);
             if (!success)
            {
                 return NotFound(); // Or an access denied view
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var userId = GetCurrentUserId();
            var recipe = _recipeService.GetRecipeByIdAndUserId(id, userId);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }
    }
}