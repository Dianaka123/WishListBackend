namespace WishListBackend.Utils.Interfaces
{
    public interface IJwtLoginService
    {
        public string CreateJwt(string id);
    }
}
