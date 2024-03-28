using WishListBackend.Models;

namespace WishListBackend.Utils.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CreateUserAsync(User user);
        public User? FindUserByEmail(string email);
        public bool ComparePassword(string passwordEncoded, string password);
        public Task<bool> TryConfirmEmail(string email);
        public void TryDeleteExpiredUser(string email);
    }
}
