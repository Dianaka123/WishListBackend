using WishListBackend.Models;

namespace WishListBackend.Utils.Interfaces
{
    public interface IJwtService
    {
        public int DefaultExperationTimeMin { get; }
        public string CreateLoginJwt(User user);
        public string CreateConfirmationEmailJwt(User user);
        public string GetEmailByToken(string token);
    }
}
