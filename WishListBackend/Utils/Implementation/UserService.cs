using WishListBackend.Models;
using WishListBackend.Other.Interfaces;
using WishListBackend.Utils.Interfaces;

namespace WishListBackend.Utils.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserContext _userDb;
        private readonly IPasswordEncoder _passwordEncoder;

        public UserService(UserContext userDb, IPasswordEncoder passwordEncoder) { 
            _userDb = userDb;
            _passwordEncoder = passwordEncoder;
        }

        public bool ComparePassword(string passwordEncoded, string password)
        {
            var decodedPassword = _passwordEncoder.Decode(passwordEncoded);
            return decodedPassword == password;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            _userDb.Add(user);
            await _userDb.SaveChangesAsync();
            return true;
        }

        public User? FindUserByEmail(string email) => _userDb.Users.FirstOrDefault(u => u.EmailAddress == email);
    }
}
