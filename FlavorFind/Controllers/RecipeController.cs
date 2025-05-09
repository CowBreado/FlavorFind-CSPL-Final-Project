using FlavorFind.BusLogic.Model;
using FlavorFind.BusLogic.Service;
using Microsoft.AspNetCore.Mvc;


namespace FlavorFind.Controllers
{
    public class RecipeController : Controller
    {
        private readonly RecipeService _recipeService = new RecipeService();

        public RecipeController()
        {
            _recipeService = new RecipeService();
        }

        public IActionResult Index()
        {
            var recipes = _recipeService.GetAllRecipes();
            
            return View(recipes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _recipeService.Add(recipe);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        public IActionResult Edit(int id)
        {
            var recipe = _recipeService.GetRecipeById(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        [HttpPost]
        public IActionResult Edit(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _recipeService.Update(recipe);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        public IActionResult Delete(int id)
        {
            var recipe = _recipeService.GetRecipeById(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _recipeService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}