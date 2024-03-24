using WishListBackend.Models;

namespace WishListBackend.Utils.Interfaces
{
    public interface IJwtService
    {
        public string CreateLoginJwt(User user);
        public string CreateConfirmationEmailJwt(User user);
    }
}
