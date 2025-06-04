using FlavorFind.BusLogic.Model;
using Dapper;

namespace FlavorFind.BusLogic.Respository
{
    public class UserRepository : GenericRepository<User>
    {
        // Custom method to find a user by email, as GenericRepository only has GetById
        public User? GetByEmail(string email)
        {
            // _connection is inherited from GenericRepository
            string query = $"SELECT * FROM Users WHERE Email = @Email";
            return _connection.QueryFirstOrDefault<User>(query, new { Email = email });
        }
    }
}