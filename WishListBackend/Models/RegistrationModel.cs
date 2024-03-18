namespace WishListBackend.Models
{
    public record class RegistrationModel()
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

    }
}
