using System;

namespace FlavorFind.BusLogic.Model
{
    public class RecipeRequest
    {
        public string? Budget { get; set; }
        public string? Ingredients { get; set; }
        public string? Preferences { get; set; }
        public string? DishType { get; set; } = "Filipino";
        public required string MealTime { get; set; }
    }
}
