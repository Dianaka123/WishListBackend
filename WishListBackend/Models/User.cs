namespace WishListBackend.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string EmailAddress { get; set; }

        public string EncryptedPassword { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

    }
}
