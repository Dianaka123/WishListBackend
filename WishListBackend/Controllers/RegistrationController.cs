using Microsoft.AspNetCore.Mvc;
using WishListBackend.Models;
using WishListBackend.Other;

namespace WishListBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly UserContext _userDb;
        private readonly IPasswordEncoder _passwordEncoder;

        public RegistrationController(ILogger<RegistrationController> logger,
            UserContext userDb,
            IPasswordEncoder passwordEncoder)
        {
            _logger = logger;
            _userDb = userDb;
            _passwordEncoder = passwordEncoder;
        }

        public record RegistrationModel(string FirstName, string LastName, string Gender, string Email, string Password, DateTime BirthDate);
        [HttpPost(Name = "Registration")]
        public async Task<IActionResult> RegisterUser(RegistrationModel userData)
        {
            var encryptedPassword = _passwordEncoder.Encode(userData.Password);

            var user = new User()
            {
                BirthDate = userData.BirthDate,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Gender = userData.Gender,
                EmailAddress = userData.Email,
                EncryptedPassword = encryptedPassword
            };

            _userDb.Add(user);
            await _userDb.SaveChangesAsync();
            return Ok();
        }
    }
}
