using FlavorFind.BusLogic.Model;
using Dapper;
using System.Collections.Generic;


namespace FlavorFind.BusLogic.Respository
{
    public class RecipeRepository : GenericRepository<Recipe>
    {
        public IEnumerable<Recipe> GetRecipesByUserId(int userId)
        {
            // _connection is inherited from GenericRepository
            string query = $"SELECT * FROM Recipe WHERE UserId = @UserId"; // Table name from Recipe model
            return _connection.Query<Recipe>(query, new { UserId = userId });
        }
    }
}