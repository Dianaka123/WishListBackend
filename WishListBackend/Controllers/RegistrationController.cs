using Microsoft.AspNetCore.Mvc;
using WishListBackend.Models;
using WishListBackend.Other.Interfaces;

namespace WishListBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly UserContext _userDb;
        private readonly IPasswordEncoder _passwordEncoder;
        private readonly IRegistrationDataValidator _dataValidator;

        public RegistrationController(ILogger<RegistrationController> logger,
            UserContext userDb,
            IPasswordEncoder passwordEncoder,
            IRegistrationDataValidator dataValidator)
        {
            _logger = logger;
            _userDb = userDb;
            _passwordEncoder = passwordEncoder;
            _dataValidator = dataValidator;
        }

        
        [HttpPost(Name = "Registration")]
        public async Task<IActionResult> RegisterUser(RegistrationModel userData)
        {

            var encryptedPassword = _passwordEncoder.Encode(userData.Password);

            if (!_dataValidator.ValidateRegistrationData(userData))
            {
                return BadRequest("Registration data is invalid.");
            }

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
