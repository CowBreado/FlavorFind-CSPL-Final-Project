using System;
using FlavorFind.BusLogic.Model;
using FlavorFind.BusLogic.Respository;

namespace FlavorFind.BusLogic.Service;

public class RecipeService
{
    private readonly RecipeRepository _recipeRepository = new RecipeRepository();

    public IEnumerable<Recipe> GetAllRecipes()
    {
        return _recipeRepository.GetAll();
    }

    public bool Add(Recipe recipe)
    {
        return _recipeRepository.Add(recipe);
    }

    public Recipe GetRecipeById(int id)
    {
        return _recipeRepository.GetById(id);
    }

    public bool Delete(int id)
    {
        return _recipeRepository.Delete(id);
    }

    public bool Update(Recipe recipe)
    {
        return _recipeRepository.Update(recipe);
    }
}
