using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using WishListBackend.Models;
using WishListBackend.Other.Interfaces;

namespace WishListBackend.Other.Implementation
{
    public class RegistrationDataValidator: IRegistrationDataValidator
    {
        private const string nameRegex = "^[A-Z][a-zA-Z]{2,}$";
        private const string emailRegex = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";
        private const string passwordRegex = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";

        private const int maxYear = 120;
        private const int minYear = 0;

        public bool ValidateRegistrationData(RegistrationModel userData)
        {
            var isFirstNameValid = ValidateStringByRegex(userData.FirstName, nameRegex);
            var isLastNameValid = ValidateStringByRegex(userData.LastName, nameRegex);
            var isEmailValid = ValidateStringByRegex(userData.Email, emailRegex);
            var isPasswordValid = ValidateStringByRegex(userData.Password, passwordRegex);
            var isBirthDateValid = ValidateBirthDate(userData.BirthDate);
            var isGendervalid = !userData.Gender.IsNullOrEmpty();

            return isFirstNameValid && isLastNameValid && isEmailValid && isPasswordValid && isBirthDateValid && isGendervalid;
        }

        public bool ValidateStringByRegex(string data, string regularExpression)
        {
            return Regex.IsMatch(data, regularExpression);
        }

        public bool ValidateBirthDate(DateTime date)
        {
            var today = DateTime.Today;
            var usersYear = today.Year - date.Year;

            return usersYear is <= maxYear and >= minYear && date < today;
        }
    }
}
