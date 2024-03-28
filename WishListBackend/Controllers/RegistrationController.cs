using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using WishListBackend.Models;
using WishListBackend.Other.Interfaces;
using WishListBackend.Utils.Interfaces;
using WishListBackend.Views;

namespace WishListBackend.Controllers
{
    [ApiController]
    [Route("api/")]
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegistrationModel userData)
        {
            var encryptedPassword = _passwordEncoder.Encode(userData.Password);

            if (!_dataValidator.ValidateRegistrationData(userData))
            {
                return BadRequest("Registration data is invalid.");
            }

            _userService.TryDeleteExpiredUser(userData.Email);

            var existedUser = _userService.FindUserByEmail(userData.Email);

            if (existedUser != null)
            {
                return BadRequest("Email exist and confirmed.");
            }

            var tokenExpirationDate = DateTime.Now.AddMinutes(_jwtService.DefaultExperationTimeMin);

            var user = new User()
            {
                BirthDate = userData.BirthDate,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Gender = userData.Gender,
                EmailAddress = userData.Email,
                EncryptedPassword = encryptedPassword,
                ExpirationDate = tokenExpirationDate,
            };

            await _userService.CreateUserAsync(user);

            var token = _jwtService.CreateConfirmationEmailJwt(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
            };

            var callback = QueryHelpers.AddQueryString(userData.ClientURL, param);
            var userEmail = new MailboxAddress("email", user.EmailAddress);
            var message = new Message(userEmail, "Email confirmation", callback);

            await _emailService.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody]string token)
        {
            var email = _jwtService.GetEmailByToken(token);
            var isSuccess = await _userService.TryConfirmEmail(email);
            if (!isSuccess)
            {
                return BadRequest("Invalid email confirmation request");
            }

            return Ok();
        }
    }
}
