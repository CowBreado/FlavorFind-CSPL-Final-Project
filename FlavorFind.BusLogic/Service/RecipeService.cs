using System;
using FlavorFind.BusLogic.Model;
using FlavorFind.BusLogic.Respository;

namespace FlavorFind.BusLogic.Service
{
    public class RecipeService
    {
        private readonly RecipeRepository _recipeRepository = new RecipeRepository();

        public IEnumerable<Recipe> GetRecipesByUserId(int userId)
        {
            return _recipeRepository.GetRecipesByUserId(userId);
        }

        public bool AddRecipeForUser(Recipe recipe, int userId)
        {
            recipe.UserId = userId; // Set the UserId before adding
            return _recipeRepository.Add(recipe);
        }

        public Recipe? GetRecipeByIdAndUserId(int recipeId, int userId)
        {
            var recipe = _recipeRepository.GetById(recipeId);
            // Verify the recipe belongs to the current user
            if (recipe != null && recipe.UserId == userId)
            {
                return recipe;
            }
            return null;
        }

        public bool UpdateRecipeForUser(Recipe recipe, int userId)
        {
            var existingRecipe = _recipeRepository.GetById(recipe.Id);
            if (existingRecipe == null || existingRecipe.UserId != userId)
            {
                return false; // Recipe not found or doesn't belong to the user
            }
            recipe.UserId = userId; // Ensure UserId is maintained
            return _recipeRepository.Update(recipe);
        }

        public bool DeleteRecipeForUser(int recipeId, int userId)
        {
            var recipe = _recipeRepository.GetById(recipeId);
            if (recipe == null || recipe.UserId != userId)
            {
                return false; // Recipe not found or doesn't belong to the user
            }
            return _recipeRepository.Delete(recipeId);
        }

        // Keep other methods if they are for general/admin access
        // public IEnumerable<Recipe> GetAllRecipes() => _recipeRepository.GetAll();
        // public Recipe GetRecipeById(int id) => _recipeRepository.GetById(id);
    }
}