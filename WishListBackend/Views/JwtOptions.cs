namespace WishListBackend.Views
{
    public record JwtOptions(string Issuer,
        string Audience,
        string SigningKey,
        int ExpirationMin);
}
