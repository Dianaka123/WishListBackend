using WishListBackend.Models;

namespace WishListBackend.Other.Interfaces
{
    public interface IRegistrationDataValidator
    {
        public bool ValidateRegistrationData(RegistrationModel userData);
        public bool ValidateStringByRegex(string data, string regularExpression);
    }
}
