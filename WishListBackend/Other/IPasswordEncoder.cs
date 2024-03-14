namespace WishListBackend.Other
{
    public interface IPasswordEncoder
    {
        string Encode(string password);
        string Decode(string encryptedPassword);
    }
}
