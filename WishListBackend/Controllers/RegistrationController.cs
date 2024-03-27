using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using WishListBackend.Models;
using WishListBackend.Other.Interfaces;
using WishListBackend.Utils.Implementation;
using WishListBackend.Utils.Interfaces;
using WishListBackend.Views;

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
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public RegistrationController(ILogger<RegistrationController> logger,
            IPasswordEncoder passwordEncoder,
            IRegistrationDataValidator dataValidator,
            IJwtService jwtService,
            IUserService userService,
            IEmailService emailService)
        {
            _logger = logger;
            _passwordEncoder = passwordEncoder;
            _dataValidator = dataValidator;
            _userService = userService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        //todo: add jwt token.
        
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
                EncryptedPassword = encryptedPassword,
                IsEmailConfirmed = false,
            };

            await _userService.CreateUserAsync(user);

            var token = _jwtService.CreateConfirmationEmailJwt(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.EmailAddress }
            };

            var callback = QueryHelpers.AddQueryString(userData.ClientURL, param);
            var userEmail = new MailboxAddress("email", user.EmailAddress);
            var message = new Message(userEmail, "Email confirmation", callback);

            await _emailService.SendEmailAsync(message);

            return Ok();
        }
    }
}
