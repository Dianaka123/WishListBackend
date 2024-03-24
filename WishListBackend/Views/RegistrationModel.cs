namespace WishListBackend.Views
{
    public record RegistrationModel(string FirstName,
        string LastName,
        DateTime BirthDate,
        string Gender,
        string Email,
        string Password,
        string? ClientURL);
}