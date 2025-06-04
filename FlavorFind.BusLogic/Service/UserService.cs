using FlavorFind.BusLogic.Model;
using FlavorFind.BusLogic.Respository;

namespace FlavorFind.BusLogic.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public User? RegisterUser(string email, string password, string? firstName = null, string? lastName = null)
        {
            if (_userRepository.GetByEmail(email) != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), // Hash the password
                FirstName = firstName,
                LastName = lastName
            };

            bool success = _userRepository.Add(user);
            return success ? _userRepository.GetByEmail(email) : null;
        }

        public User? LoginUser(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null; 
            }
            return user;
        }
    }
}