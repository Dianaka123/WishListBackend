namespace WishListBackend.Other.Interfaces
{
    public interface IPasswordEncoder
    {
        string Encode(string password);
        string Decode(string encryptedPassword);
    }
}
