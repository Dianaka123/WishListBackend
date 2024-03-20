using Microsoft.AspNetCore.Mvc;
using WishListBackend.Models;
using WishListBackend.Other.Interfaces;
using WishListBackend.Utils.Implementation;
using WishListBackend.Utils.Interfaces;

namespace WishListBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IPasswordEncoder _passwordEncoder;
        private readonly IRegistrationDataValidator _dataValidator;
        private readonly IUserService _userService;

        public RegistrationController(ILogger<RegistrationController> logger,
            UserContext userDb,
            IPasswordEncoder passwordEncoder,
            IRegistrationDataValidator dataValidator,
            IUserService userService)
        {
            _logger = logger;
            _passwordEncoder = passwordEncoder;
            _dataValidator = dataValidator;
            _userService = userService;
        }

        
        [HttpPost(Name = "registration")]
        public async Task<IActionResult> RegisterUser(RegistrationModel userData)
        {
            var encryptedPassword = _passwordEncoder.Encode(userData.Password);

            if (!_dataValidator.ValidateRegistrationData(userData))
            {
                return BadRequest("Registration data is invalid.");
            }

            if(_userService.FindUserByEmail(userData.Email) != null)
            {
                return BadRequest("Email exist.");
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

            await _userService.CreateUserAsync(user);
            
            return Ok();

        }


    }
}
