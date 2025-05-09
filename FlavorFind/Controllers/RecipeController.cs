using FlavorFind.BusLogic.Model;
using FlavorFind.BusLogic.Respository;
using Microsoft.AspNetCore.Mvc;

namespace FlavorFind.Controllers
{
    public class RecipeController : Controller
    {
        private readonly RecipeRepository _recipeRepository;

        public RecipeController()
        {
            _recipeRepository = new RecipeRepository();
        }

        public IActionResult Index()
        {
            var recipes = _recipeRepository.GetAll();
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
                _recipeRepository.Add(recipe);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        public IActionResult Edit(int id)
        {
            var recipe = _recipeRepository.GetById(id);
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
                _recipeRepository.Update(recipe);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        public IActionResult Delete(int id)
        {
            var recipe = _recipeRepository.GetById(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _recipeRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}